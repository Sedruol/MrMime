
import logging
import time
import ast

from tf_pose import common
import cv2
import numpy as np
from tf_pose.estimator import TfPoseEstimator
from tf_pose.networks import get_graph_path, model_wh

from tf_pose.lifting.prob_model import Prob3dPose
from tf_pose.lifting.draw import plot_pose

#Dataset
#import scipy.io
#dataset = scipy.io.loadmat('joints.mat')

#modelo
e = TfPoseEstimator(get_graph_path('mobilenet_thin'), target_size=(432, 368))
#imagenes
image = common.read_imgfile('./dataset_images/im0001.jpg', None, None)
#inferencia
t = time.time()
w= 0
h = 0
humans = e.inference(image, resize_to_default=(w > 0 and h > 0), upsample_size=1.0)
elapsed = time.time() - t
print('inference image: %s in %.4f seconds.' % ('im0001', elapsed))
#print(humans)