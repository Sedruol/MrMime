#Script used to make statistics using the test dataset
import os
import math

import pandas as pd
import cv2
import numpy as np
import matplotlib.pyplot as plt
import statistics as stat

from tf_pose import common
from tf_pose.estimator import TfPoseEstimator
from tf_pose.networks import get_graph_path, model_wh

from tf_pose.lifting.prob_model import Prob3dPose
from tf_pose.lifting.draw import plot_pose

import tensorflow as tf
from keras.models import model_from_json

def prom(a):
    res = 0
    c = len(a)
    for i in a:
        res += i
    return res/c

def distancia2D(v1, v2):
    a = pow(v1[0] - v2[0],2)
    b = pow(v1[1] - v2[1],2)
    res = a + b
    return math.sqrt(res)

def distancia(v1, v2):
    a = pow(v1[0] - v2[0],2)
    b = pow(v1[1] - v2[1],2)
    c = pow(v1[2] - v2[2],2)
    res = a + b + c
    return math.sqrt(res)
    

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
def puntoSegunDireccion(punto, direccion):
    a = [round(punto[0]+direccion[0], 3),round(punto[1]+direccion[1], 3),round(punto[2]+direccion[2], 3)]
    return a
def printBrazo(p1,p2,p3):
    fig = plt.figure()
    ax1 = plt.axes(projection='3d')
    ax1.set(xlim =(-4,4),ylim =(-4,4),zlim =(-4,4))
    x = np.array([[p1[0],p2[0],p3[0]]])
    y = np.array([[p1[1],p2[1],p3[1]]])
    z = np.array([[p1[2],p2[2],p3[2]]])
    ax1.plot_wireframe(x, y, z)
    ax1.scatter(x, y, z)
    plt.show()
    
def Brazo(hombro, codo, mano):
    a = vectorUnitario(codo, hombro)
    b = vectorUnitario(mano, codo)
    p1 = [0,0,0]
    p2 = puntoSegunDireccion(p1, a)
    p3 = puntoSegunDireccion(p2, b)
    printBrazo(p1,p2,p3)
    
def BrazoSegunVectores(v1,v2):
    p1 = [0,0,0]
    p2 = puntoSegunDireccion(p1, v1)
    p3 = puntoSegunDireccion(p2, v2)
    return p3

if __name__ == '__main__':
        
    model = 'cmu'
    resolution = '432x368'
    #model
    w, h = model_wh(resolution)
    e = TfPoseEstimator(get_graph_path(model), target_size=(w, h))
    #diccionarios
    
    dir1 = {"0": [0,0,-1],"1bl":[0.577,0.577,-0.577],"1l": [0.707,0,-0.707],"1fl":[0.577,-0.577,-0.577],"1f":[0,-0.707,-0.707],"1fr":[-0.577,-0.577,-0.577],"1r":[-0.707,0,-0.707],"1br":[-0.577,0.577,-0.577],"2bl":[0.707,0.707,0],"2l":[1,0,0],"2fl":[0.707,-0.707,0],"2f":[0,-1,0],"2fr":[-0.707,-0.707,0],"2r":[-1,0,0],"3bl":[0.577,0.577,0.577],"3l":[0.707,0,0.707],"3fl":[0.577,-0.577,0.577],"3f":[0,-0.707,0.707],"3fr":[-0.577,-0.577,0.577],"3r":[-0.707,0,0.707],"3b":[0,0.707,0.707],"4":[0,0,1]}
    dir2 = {"0": 0,"1bl":1,"1l": 2,"1fl":3,"1f":4,"1fr":5,"1r":6,"1br":7,"2bl":8,"2l":9,"2fl":10,"2f":11,"2fr":12,"2r":13,"3bl":14,"3l":15,"3fl":16,"3f":17,"3fr":18,"3r":19,"3b":20,"4":21}
    dir3 = []
    for key in dir2:
      dir3.append(key)
    ppdir = {"1_1": [0, 0, -2],"1_2":[0, -1.414, -1.414],"1_3":[0, -2, 0],
         "2_1": [0, -2, 0],"2_2":[0, -1.414, 1.414],"2_3":[0, 0, 2],
         "3_1": [0, 0, -2],"3_2":[-1.414, 0, -1.414],"3_3":[-2, 0, 0],
         "4_1": [-2, 0, 0],"4_2":[-1.414, 0, 1.414],"4_3":[0, 0, 2],
         "5_1": [-1, -1, 0],"5_2":[0, -1.414, 0],"5_3":[1, -1, 0],
         "6_1": [1, -1, 0],"6_2":[0.707, -1, 0.707],"6_3":[0, -1, 1],
         "7_1": [0, 0, -2],"7_2":[-0.707, 0, -1.707],"7_3":[-1, 0, -1],
         "8_1": [-0.293, -0.707, 0],"8_2":[0, -1.414, 0],"8_3":[0, -2, 0],
         "9_1": [-0.707, -0.707, 1],"9_2":[-1.284, -1.284, 0.577],"9_3":[-1.414, -1.414, 0],
         "10_1": [-1.154, -1.154, 0.0],"10_2":[-1.284, -1.284, -0.577],"10_3":[-1.154, -1.154, -1.154]}
    #modelos
    json_file = open('./models2/model.json', 'r')
    loaded_model_json = json_file.read()
    json_file.close()
    model1 = model_from_json(loaded_model_json)
    model1.load_weights("./models2/model.h5")
      
    json_file = open('./models2/model2.json', 'r')
    loaded_model_json = json_file.read()
    json_file.close()
    model2 = model_from_json(loaded_model_json)
    model2.load_weights("./models2/model2.h5")

    arrsubs = ["a","b","c","j","l","m","n","r","s","y"]
    for k in arrsubs:

        print("--------------------------------------------------------------------------------------------")
        print(k)
        print("--------------------------------------------------------------------------------------------")
        
        #read_images
        images = {}
        folder = "C:/pruebas/" + k#cambiar aqui por la carpeta con el dataset
        imagesFileNames = os.listdir(folder)
        
        
        counter = 0
        arrDist = []
        for i in imagesFileNames:
            image = common.read_imgfile(folder + "/" + i , None, None)
            codigoImagen = i.split(sep = ".")[0]
            images[codigoImagen] = image
            
            #prediccion de pose 2d
            humans = e.inference(image, resize_to_default=(w > 0 and h > 0), upsample_size=4.0)
            '''
            print("-----------------------------------------")
            print(humans)
            print("-----------------------------------------")
            '''
            
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

            #obtener vectores
            pointsCount = len(pose_3d[0][0])

            mano = [pose_3d[0][0][pointsCount-1],pose_3d[0][1][pointsCount-1],pose_3d[0][2][pointsCount-1]]
            codo = [pose_3d[0][0][pointsCount-2],pose_3d[0][1][pointsCount-2],pose_3d[0][2][pointsCount-2]]
            hombro = [pose_3d[0][0][pointsCount-3],pose_3d[0][1][pointsCount-3],pose_3d[0][2][pointsCount-3]]
            v1= vectorUnitario(codo, hombro)
            v2= vectorUnitario(mano,codo)
            
            try:
                mano2d = [humans[0].body_parts[4].x*-100,humans[0].body_parts[4].y*-100]
                codo2d = [humans[0].body_parts[3].x*-100,humans[0].body_parts[3].y*-100]
                hombro2d = [humans[0].body_parts[2].x*-100,humans[0].body_parts[2].y*-100]
                sv1 = vectorUnitario(codo2d, hombro2d)
                sv2 = vectorUnitario(mano2d, codo2d)
                sv1[0] = sv1[0]*-1
                sv2[0] = sv2[0]*-1
            except:
                print("hubo problemas en la prediccion 2d")
                sv1 = [v1[0]*-1,v1[2]]
                sv2 = [v2[0]*-1,v2[2]]
            
            
            sePred = [sv1[0],sv1[1],v1[0],v1[1],v1[2]]
            ehPred = [sv2[0],sv2[1],v2[0],v2[1],v2[2]]
            '''
            print("--------------------------------------------------------------------------------------------")
            print(str(sv1[0]) + " " +str(sv1[1]) + " "+str(v1[0]) + " "+str(v1[1]) + " "+str(v1[2]) + " " )
            
            print(str(sv2[0]) + " " +str(sv2[1]) + " "+str(v2[0]) + " "+str(v2[1]) + " "+str(v2[2]) + " " )
            print("--------------------------------------------------------------------------------------------")
            '''
            puntoP3D = BrazoSegunVectores(v1, v2)

            #predecir punto
            right_se = model1.predict_classes([[sePred]])
            ehPred.append(right_se)
            right_eh = model2.predict_classes([[ehPred]])
            if(dir3[right_eh[0]] == "4" or dir3[right_eh[0]] == "3b"):
                if(distancia2D(mano2d,codo2d)<=7):
                    right_eh[0] = 11
                
            predictionR = dir3[right_se[0]]+"_"+dir3[right_eh[0]]
            vp1 = dir1[dir3[right_se[0]]]
            vp2 = dir1[dir3[right_eh[0]]]
            puntoPredecido = BrazoSegunVectores(vp1, vp2)
            puntoDefinido = ppdir[codigoImagen]

            #distancia
            dist = distancia(puntoDefinido,puntoPredecido)
            arrDist.append(dist)
            counter+=1

            #segunda distancia
            dist2 = distancia(puntoDefinido, puntoP3D)
            dist3 = distancia(puntoPredecido, puntoP3D)
            
            
                
            linea = "codigo: " + codigoImagen + " prediccion: " + predictionR + " distancia: " + str(dist) + " distanciaPuntoDef: " + str(dist2) + " distanciaPuntoPred: " + str(dist3)
            print(linea)
            print(puntoPredecido)
            print(puntoDefinido)
            if(counter>=3):
                print("===================")
                print("distProm:")
                print(prom(arrDist))
                print("===================")
                arrDist = []
                counter = 0
           

        
        

        

        
        
        

        
    
        

    
