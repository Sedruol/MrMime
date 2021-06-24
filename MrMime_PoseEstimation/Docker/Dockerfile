FROM ubuntu:16.04

#installs
#RUN apt-get -y update && apt-get install -y --no-install-recommends \
 #        wget \
 #        python3.6 \
  #       nginx \
   #      ca-certificates \
    #&& rm -rf /var/lib/apt/lists/*

RUN apt-get update 
RUN apt-get install -y wget
RUN apt-get install -y nginx
RUN apt-get install -y swig 
RUN apt-get install -y git
RUN apt-get install -y python3-pip 
RUN pip3 install numpy==1.16.2 flask gevent gunicorn 
RUN rm -rf /var/lib/apt/lists/


#path variables

ENV PYTHONUNBUFFERED=TRUE
ENV PYTHONDONTWRITEBYTECODE=TRUE
ENV PATH="/opt/program:${PATH}"

WORKDIR /opt/program
RUN cd /opt/program

RUN git clone https://github.com/DiegoJohnsonUPC/Pose_Estimation_3D.git 
RUN find /opt/program/Pose_Estimation_3D/requirements.txt
RUN cd Pose_Estimation_3D
RUN python3 -c 'import os;'
RUN python3 run.py
RUN pip3 install -r requirements.txt 
RUN pip3 install matplotlib 
RUN cd tf_pose 
RUN cd pafprocess 
RUN swig -python -c++ pafprocess.i 
RUN python3 setup.py build_ext --inplace 
RUN cd ..
RUN pip3 install tensorflow tensorflow-gpu  
RUN python3 -c 'import tensorflow as tf; print(tf.__version__)'
RUN python3 run.py --model=mobilenet_thin --resize=432x368 --image=./dataset_images/im0001.jpg 

