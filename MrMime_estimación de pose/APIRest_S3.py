from flask import Flask,jsonify
import boto3

import argparse
import logging
import time
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
from collections import Counter


def moda(arr):
  c = Counter(arr)
  return [k for k, v in c.items() if v == c.most_common(1)[0][1]][0]
  
def transformKeyToPoint(key):
  keys = key.split(sep='_',maxsplit=2)
  direc = {"0": [0,0,-1],"1bl":[0.577,0.577,-0.577],"1l": [0.707,0,-0.707],"1fl":[0.577,-0.577,-0.577],"1f":[0,-0.707,-0.707],"1fr":[-0.577,-0.577,-0.577],"1r":[-0.707,0,-0.707],"1br":[-0.577,0.577,-0.577],"2bl":[0.707,0.707,0],"2l":[1,0,0],"2fl":[0.707,-0.707,0],"2f":[0,-1,0],"2fr":[-0.707,-0.707,0],"2r":[-1,0,0],"3bl":[0.577,0.577,0.577],"3l":[0.707,0,0.707],"3fl":[0.577,-0.577,0.577],"3f":[0,-0.707,0.707],"3fr":[-0.577,-0.577,0.577],"3r":[-0.707,0,0.707],"3b":[0,0.707,0.707],"4":[0,0,1]}
  a = direc[keys[0]]
  b = direc[keys[1]]
  p = BrazoSegunVectores(a,b);
  return p

def magnitud(v):
    res = pow(v[0],2) + pow(v[1],2) + pow(v[2],2)
    res = math.sqrt(res)
    return res

def vectorDirection(v1,v2):
    res = []
    res.append(v1[0]-v2[0])
    res.append(v1[1]-v2[1])
    res.append(v1[2]-v2[2])
    return res
def vectorUnitario(v1, v2):
    v = vectorDirection(v1,v2)
    m = magnitud(v)
    v[0] = v[0]/m
    v[1] = v[1]/m
    v[2] = v[2]/m
    return v
def puntoSegunDireccion(punto, direccion):
    a = [punto[0]+direccion[0],punto[1]+direccion[1],punto[2]+direccion[2]]
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


def write_vector_data(arr, archi, humans):
    pointsCount = len(arr[0][0])
    #mano derecha
    mano = [arr[0][0][pointsCount-1],arr[0][1][pointsCount-1],arr[0][2][pointsCount-1]]
    codo = [arr[0][0][pointsCount-2],arr[0][1][pointsCount-2],arr[0][2][pointsCount-2]]
    hombro = [arr[0][0][pointsCount-3],arr[0][1][pointsCount-3],arr[0][2][pointsCount-3]]
    v1= vectorUnitario(codo, hombro)
    v2= vectorUnitario(mano,codo)
    #2d derecha
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
    linea = str(sv1[0]) + " " + str(sv1[1]) + " " + str(sv2[0]) + " " + str(sv2[1]) + " " + str(v1[0]) +" "+ str(v1[1])+" "+str(v1[2])+" "+str(v2[0]) +" "+ str(v2[1])+" "+str(v2[2])
    #mano izquierda
    mano = [arr[0][0][pointsCount-4],arr[0][1][pointsCount-4],arr[0][2][pointsCount-4]]
    codo = [arr[0][0][pointsCount-5],arr[0][1][pointsCount-5],arr[0][2][pointsCount-5]]
    hombro = [arr[0][0][pointsCount-6],arr[0][1][pointsCount-6],arr[0][2][pointsCount-6]]
    v1= vectorUnitario(codo, hombro)
    v2= vectorUnitario(mano,codo)
    #2d izquierda
    try:
      mano2d = [humans[0].body_parts[7].x*-100,humans[0].body_parts[7].y*-100]
      codo2d = [humans[0].body_parts[6].x*-100,humans[0].body_parts[6].y*-100]
      hombro2d = [humans[0].body_parts[5].x*-100,humans[0].body_parts[5].y*-100]
      sv1 = vectorUnitario(codo2d, hombro2d)
      sv2 = vectorUnitario(mano2d, codo2d)
      sv1[0] = sv1[0]*-1
      sv2[0] = sv2[0]*-1
    except:
      print("hubo problemas en la prediccion 2d")
      sv1 = [v1[0]*-1,v1[2]]
      sv2 = [v2[0]*-1,v2[2]]
    #linea
    linea += str(sv1[0]) + " " + str(sv1[1]) + " " + str(sv2[0]) + " " + str(sv2[1]) + " " + str(v1[0]) +" "+ str(v1[1])+" "+str(v1[2])+" "+str(v2[0]) +" "+ str(v2[1])+" "+str(v2[2])
    archi.write(linea)
    archi.write('\n')

#====================================================================================================================================================

app = Flask(__name__)
s3 = boto3.client('s3')
#model
model = 'cmu'
resolution = '432x368'
w, h = model_wh(resolution)
e = TfPoseEstimator(get_graph_path(model), target_size=(w, h))

#Obtener Lista de Videos
@app.route('/videos',methods = ['GET'])
def showVideo():
  return jsonify(videos)

#Descargar y Subir Resultados al S3
@app.route('/<string:video_name>',methods = ['GET'])
def uploadResult(video_name):
  #videosFound = [video for video in videos if video["name"] == video_name]
  bucket_name = 'tesis-resources'
  #if (len(videosFound)>0):
  try:
    s3.download_file(bucket_name, video_name, './videos_API' + "/" + video_name)
    
  except:
    return jsonify({"message": "Video not found"})
  
  
  
  

 

  cap = cv2.VideoCapture('./videos_API/'+ video_name)#ACA VA EL VIDEO
  results = []
  #iterators
  i = 0
  ci = 0
  #file
  archi=open("temp.txt","w")
  archi.write("dse1_x dse1_y deh1_x deh1_y se1_x se1_y se1_z eh1_x eh1_y eh1_z dse2_x dse2_y deh2_x deh2_y se2_x se2_y se2_z eh2_x eh2_y eh2_z")
  archi.write('\n')

  #diccionarios
  dir2 = {"0": 0,"1bl":1,"1l": 2,"1fl":3,"1f":4,"1fr":5,"1r":6,"1br":7,"2bl":8,"2l":9,"2fl":10,"2f":11,"2fr":12,"2r":13,"3bl":14,"3l":15,"3fl":16,"3f":17,"3fr":18,"3r":19,"3b":20,"4":21}
  dir3 = []
  for key in dir2:
    dir3.append(key)
    
  #modelos
  json_file = open('./neural_networks2/model.json', 'r')
  loaded_model_json = json_file.read()
  json_file.close()
  model1 = model_from_json(loaded_model_json)
  model1.load_weights("./neural_networks2/model.h5")
  
  #print(model1.predict_classes([[[0,0,-1]]])[0])
  json_file = open('./neural_networks2/model2.json', 'r')
  loaded_model_json = json_file.read()
  json_file.close()
  model2 = model_from_json(loaded_model_json)
  model2.load_weights("./neural_networks2/model2.h5")

  #aqui se itera en el video para obtener las  imagenes
   
  if cap.isOpened() is False:
    print("Error opening video stream or file")
      
  while cap.isOpened():
    ret_val, image = cap.read() 
    if not ret_val:
      print("finalizado")
      break
    humans = e.inference(image, resize_to_default=(w > 0 and h > 0), upsample_size=4.0)
    if not True:
      image = np.zeros(image.shape)
    image = TfPoseEstimator.draw_humans(image, humans, imgcopy=False)
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
    write_vector_data(pose_3d, archi,humans)
  archi.close()
    
  #leemos el temp    
  df = pd.read_csv(r"./temp.txt",sep=' ')
  #predecimos con la red neuronal
  tempArrR = []
  posesR = []
  tempArrL = []
  posesL = []
  for i in df.index:
    #right
    rse = [df["dse1_x"][i],df["dse1_y"][i],df["se1_x"][i],df["se1_y"][i],df["se1_z"][i]]
    right_se = model1.predict_classes([[rse]])
    reh = [df["deh1_x"][i],df["deh1_y"][i],df["eh1_x"][i],df["eh1_y"][i],df["eh1_z"][i],right_se]
    right_eh = model2.predict_classes([[reh]])
    try:
      predictionR = dir3[right_se[0]]+"_"+dir3[right_eh[0]]
    except:
      print("error cargando en dir3 " +str(right_se[0])+" " + str(right_eh[0]))
    tempArrR.append(predictionR)
    #left
    lse = [df["dse2_x"][i],df["dse2_y"][i],df["se2_x"][i]*-1,df["se2_y"][i],df["se2_z"][i]]
    left_se = model1.predict_classes([[lse]])
    leh = [df["deh2_x"][i],df["deh2_y"][i],df["eh2_x"][i]*-1,df["eh2_y"][i],df["eh2_z"][i],left_se]
    left_eh = model2.predict_classes([[leh]])
    predictionL = dir3[left_se[0]]+"_"+dir3[left_eh[0]]
    tempArrL.append(predictionL)
    if((i/10).is_integer() and  i != 0):
      modaR = moda(tempArrR)
      posesR.append(modaR)
      tempArrR = []
      modaL = moda(tempArrL)
      posesL.append(modaL)
      tempArrL = []
  try:
    modaR = moda(tempArrR)
    posesR.append(modaR)
    modaL = moda(tempArrL)
    posesL.append(modaL)
  except:
    print("problema")
  #se escribe el nuevo archivo
  S = "vid" #esto se cambia por el nombre del video
  archi2=open("resultado.txt","w")
  for it in posesR:
    vector = transformKeyToPoint(it)
    line = str(vector[0])+","+str(vector[1])+","+str(vector[2])
    archi2.write(line)
    archi2.write('\n')
  archi2.write('a')
  for it in posesL:
    vector = transformKeyToPoint(it)
    line = str(vector[0])+","+str(vector[1])+","+str(vector[2])
    archi2.write('\n')
    archi2.write(line)
        
  archi2.close()
   
  new_video_name = video_name.split(sep = '.')
  
  response = s3.upload_file(r'./resultado.txt',bucket_name,'R_'+ new_video_name[0]+".txt")
  return jsonify({"message": "Complete"})

if __name__ == '__main__':
  app.run(debug=True,port=5000)
