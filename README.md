# Folder Structure
1. MrMime
- Here is the application developed in Unity.
2. MrMime_PoseEstimation
- Here are the files used for the pose estimate and the API_Rest.
3. Dataset_Train
- Here are the files that conformed the training's dataset.
4. Dataset_Test
- Here are the files that conformed the test's dataset.
# Important
Before running the Unity application, you need to run the "APIRest_S3.py" as follows:
1. You must enter the folder "MrMime_PoseEstimation". Also, you must open the command terminal, you can use an environment in anaconda or work on the computer directly.

![Texto alternativo](https://github.com/Sedruol/Images_TP/blob/main/pose%20estimation.png)

2. Then, in the terminal, all the necessary libraries must be installed for correct operation with the command “pip install –r requirements.txt”.

![Texto alternativo](https://github.com/Sedruol/Images_TP/blob/main/requirements.png)

3. In addition, it is necessary to build the libraries in c ++ for the post process in the OpenPose algorithm, so we change the directory 
to "tf_pose" with "cd tf_pose" and then to "pafprocess" with "cd pafprocess".

![Texto alternativo](https://github.com/Sedruol/Images_TP/blob/main/build_process.png)

4. Now, execute the command "swig -python -c++ pafprocess.i".

![Texto alternativo](https://github.com/Sedruol/Images_TP/blob/main/pafprocess.png)

5. The folder should look like this.

![Texto alternativo](https://github.com/Sedruol/Images_TP/blob/main/folder.png)

6. Once you have this ready and making sure of all the previous requirements, your test environment is ready and you can run the test scripts and the API_REST. 
To do this, you must execute the command: "python APIRest_S3.py" and it should look like this.

![Texto alternativo](https://github.com/Sedruol/Images_TP/blob/main/api%20rest.png)

7. Finally, this window must be kept open when using the application to process the videos sent to the server.
