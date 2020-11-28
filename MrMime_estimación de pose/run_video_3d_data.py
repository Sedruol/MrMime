import argparse
import logging
import time

import cv2
import numpy as np
import matplotlib.pyplot as plt

from tf_pose import common
from tf_pose.estimator import TfPoseEstimator
from tf_pose.networks import get_graph_path, model_wh

from tf_pose.lifting.prob_model import Prob3dPose
from tf_pose.lifting.draw import plot_pose

logger = logging.getLogger('TfPoseEstimator-Video')
logger.setLevel(logging.DEBUG)
ch = logging.StreamHandler()
ch.setLevel(logging.DEBUG)
formatter = logging.Formatter('[%(asctime)s] [%(name)s] [%(levelname)s] %(message)s')
ch.setFormatter(formatter)
logger.addHandler(ch)

fps_time = 0


def write_data(arr, archi):
    for i in range(0, len(arr[0][0])):
        linea = str(arr[0][0][i])+" "+str(arr[0][1][i])+" "+str(arr[0][2][i])
        #print(linea)
        archi.write(linea)
        archi.write('\n')
    archi.write(".")
    archi.write('\n')



if __name__ == '__main__':
    #parser
    parser = argparse.ArgumentParser(description='tf-pose-estimation Video')
    parser.add_argument('--video', type=str, default='')
    parser.add_argument('--resolution', type=str, default='432x368', help='network input resolution. default=432x368')
    parser.add_argument('--model', type=str, default='mobilenet_thin', help='cmu / mobilenet_thin / mobilenet_v2_large / mobilenet_v2_small')
    parser.add_argument('--show-process', type=bool, default=False,
                        help='for debug purpose, if enabled, speed for inference is dropped.')
    parser.add_argument('--resize-out-ratio', type=float, default=4.0,
                        help='if provided, resize heatmaps before they are post-processed. default=1.0')
    parser.add_argument('--showBG', type=bool, default=True, help='False to show skeleton only.')
    args = parser.parse_args()
    #model
    logger.debug('initialization %s : %s' % (args.model, get_graph_path(args.model)))
    w, h = model_wh(args.resolution)
    e = TfPoseEstimator(get_graph_path(args.model), target_size=(w, h))
    cap = cv2.VideoCapture(args.video)
    results = []
    #iterators
    i = 0
    ci = 0
    #file
    archi=open("output.txt","w") 
    if cap.isOpened() is False:
        print("Error opening video stream or file")
    while cap.isOpened():
        
        ret_val, image = cap.read()
        
        if not ret_val:
            print("finalizado")
            break
        humans = e.inference(image, resize_to_default=(w > 0 and h > 0), upsample_size=args.resize_out_ratio)
        #humans = e.inference(image)
        if not args.showBG:
            image = np.zeros(image.shape)
        image = TfPoseEstimator.draw_humans(image, humans, imgcopy=False)
        #jh = humans[0].body_parts[0].x
        #print(jh)
        #cv2.putText(image, "FPS: %f" % (1.0 / (time.time() - fps_time)), (10, 10),  cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 255, 0), 2)
        
        #cv2.putText(image, "FPS: %f" % (jh, (10, 10),  cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 255, 0), 2)
        #cv2.imshow('tf-pose-estimation result', image)
        #fps_time = time.time()
        logger.info('3d lifting initialization.')
        poseLifting = Prob3dPose('./tf_pose/lifting/models/prob_model_params.mat')

        image_h, image_w = image.shape[:2]
        standard_w = 640
        standard_h = 480

        pose_2d_mpiis = []
        visibilities = []
        for human in humans:
            pose_2d_mpii, visibility = common.MPIIPart.from_coco(human)
            pose_2d_mpiis.append([(int(x * standard_w + 0.5), int(y * standard_h + 0.5)) for x, y in pose_2d_mpii])
            visibilities.append(visibility)

        pose_2d_mpiis = np.array(pose_2d_mpiis)
        visibilities = np.array(visibilities)
        transformed_pose2d, weights = poseLifting.transform_joints(pose_2d_mpiis, visibilities)
        pose_3d = poseLifting.compute_3d(transformed_pose2d, weights)
        i+=1
        
        print(i)
        if(ci<5):
            ci+=1
        else:
            ci = 0
            write_data(pose_3d, archi)
            for i, single_3d in enumerate(pose_3d):
                plot_pose(single_3d)
            plt.show()
        

    cv2.destroyAllWindows()
    logger.debug('finished+')



    