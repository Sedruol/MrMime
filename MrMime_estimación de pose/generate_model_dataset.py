#Script used for create a dataset based on the image dataset proposed in the paper

import argparse
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

import os
import matplotlib.pyplot as plt

import math
import matplotlib.pyplot as plt
import numpy as np
def magnitud(v):
    res = 0
    for i in range(0, len(v)):
        res += pow(v[i],2) 
    res = math.sqrt(res)
    return res

def vectorDirection(v1,v2):
    res = []
    for i in range(0, len(v1)):
        res.append(v1[i]-v2[i])
    return res
def vectorUnitario(v1, v2):
    v = vectorDirection(v1,v2)
    m = magnitud(v)
    for i in range(0, len(v1)):
        v[i] = v[i]/m
    return v

archi=open("datasetModelos_2.txt","w")
types = os.listdir("C:/dataset")

w = 432
h = 368
e = TfPoseEstimator(get_graph_path("cmu"), target_size=(432, 368))
count = 0
archi.write("label hand2dX hand2dY elbow2dX elbow2dY shoulder2dX shoulder2dY shoulder_elbow2dX shoulder_elbow2dY elbow_hand2dX elbow_hand2dY handX handY handZ elbowX elbowY elbowZ shoulderX shoulderY shoulderZ shoulder_elbowX shoulder_elbowY shoulder_elbowZ elbow_handX elbow_handY elbow_handZ")
archi.write('\n')
for i in types: #grupo
    types2 = os.listdir("C:/dataset/"+i)#cambiar por ruta donde se tenga el dataset
    for j in types2:  #etiqueta
        folder = "C:/dataset/"+i+"/"+j
        imagesFileNames = os.listdir(folder)
        for k in imagesFileNames: #nombre
            count+=1
            print(count)
            try:
                image = common.read_imgfile("C:/dataset/"+i+"/"+j+"/"+k, None, None) 
                #prediccion de pose 2d
                humans = e.inference(image, resize_to_default=(w > 0 and h > 0), upsample_size=4.0)
                #prediccion de pose 3d
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
                #write 2d vectors 3d vectors i 
                resultado = ""
                pointsCount = len(pose_3d[0][0])
                #etiqueta
                etiqueta = j

                #2d=========================
                mano2dX = humans[0].body_parts[7].x*-100
                mano2dY = humans[0].body_parts[7].y*-100
                mano2d = [mano2dX, mano2dY]

                codo2dX = humans[0].body_parts[6].x*-100
                codo2dY = humans[0].body_parts[6].y*-100
                codo2d = [codo2dX, codo2dY]

                hombro2dX = humans[0].body_parts[5].x*-100
                hombro2dY = humans[0].body_parts[5].y*-100
                hombro2d = [hombro2dX, hombro2dY]

                hombro_codo2d = vectorUnitario(codo2d,hombro2d)
                codo_mano2d = vectorUnitario(mano2d,codo2d)
                
                #3d=========================
                manoX = pose_3d[0][0][pointsCount-4]
                manoY = pose_3d[0][1][pointsCount-4]
                manoZ = pose_3d[0][2][pointsCount-4]
                mano = [manoX, manoY, manoZ]

                codoX = pose_3d[0][0][pointsCount-5]
                codoY = pose_3d[0][1][pointsCount-5]
                codoZ = pose_3d[0][2][pointsCount-5]
                codo = [codoX, codoY, codoZ]

                hombroX = pose_3d[0][0][pointsCount-6]
                hombroY = pose_3d[0][1][pointsCount-6]
                hombroZ = pose_3d[0][2][pointsCount-6]
                hombro = [hombroX, hombroY, hombroZ]

                hombro_codo = vectorUnitario(codo,hombro)
                codo_mano = vectorUnitario(mano, codo)


                linea = etiqueta + " " + str(mano2d[0])+" " + str(mano2d[1])+" "+ str(codo2d[0])+ " " + str(codo2d[1])+ " "+ str(hombro2d[0])+ " " +str(hombro2d[1])+" "+ str(hombro_codo2d[0])+ " " + str(hombro_codo2d[1])+" "+ str(codo_mano2d[0])+ " " + str(codo_mano2d[1])+" " +str(mano[0])+ " " + str(mano[1])+ " " +str(mano[2]) + " " + str(codo[0])+ " " + str(codo[1])+ " " +str(codo[2]) + " " + str(hombro[0])+ " " + str(hombro[1])+ " " +str(hombro[2]) + " " + str(hombro_codo[0]*-1)+ " " + str(hombro_codo[1])+ " " +str(hombro_codo[2])+ " " + str(codo_mano[0]*-1)+ " " + str(codo_mano[1])+ " " +str(codo_mano[2])
                archi.write(linea)
                archi.write('\n')
            except:
              print("Failed to predict")
            
