#script used to generate the posible combinations of arm vectors
import math
import matplotlib.pyplot as plt
import numpy as np

class Direction:

    def __init__(self,nombre,direccion):

        self.n = nombre
        self.d = direccion
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
mano = [-562.7040271377562,-39.24209119008578,341.1336609700456]
codo = [-376.2574128602313,83.46824224110065,368.4048020866041]
hombro = [-144.13440036863216,93.07545529529807,462.12986826590975]
#print(vectorUnitario(codo, hombro))
#print(vectorUnitario(mano, codo))
def puntoSegunDireccion(punto, direccion):
    a = [punto[0]+direccion[0],punto[1]+direccion[1],punto[2]+direccion[2]]
    return a
def printBrazo(p1,p2,p3):
    fig = plt.figure()
    ax1 = plt.axes(projection='3d')
    ax1.set(xlim =(-2,2),ylim =(-2,2),zlim =(-2,2))
    x = np.array([[p1[0],p2[0],p3[0]]])
    y = np.array([[p1[1],p2[1],p3[1]]])
    z = np.array([[p1[2],p2[2],p3[2]]])
    ax1.plot_wireframe(x, y, z)
    ax1.scatter(x, y, z, color = ['red','green','blue'])
    ax1.view_init(15,90)
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
    printBrazo(p1,p2,p3)
def vectorUnitarioVector(v):
    m = magnitud(v)
    v[0] = v[0]/m
    v[1] = v[1]/m
    v[2] = v[2]/m
    return v   
    
#printBrazo(hombro,codo,mano)
#Brazo(hombro,codo,mano)
direcciones = []
direcciones.append(Direction("abajo","[0,0,-1]"))

direcciones.append(Direction("abajo_atras_izquierda","[0.577,0.577,-0.577]"))
direcciones.append(Direction("abajo_izquierda","[0.707,0,-0.707]"))
direcciones.append(Direction("abajo_frente_izquierda","[0.577,-0.577,-0.577]"))
direcciones.append(Direction("abajo_frente","[0,-0.707,-0.707]"))
direcciones.append(Direction("abajo_frente_derecha","[-0.577,-0.577,-0.577]"))
direcciones.append(Direction("abajo_derecha","[-0.707,0,-0.707]"))
direcciones.append(Direction("abajo_atras_derecha","[-0.577,0.577,-0.577]"))
direcciones.append(Direction("abajo_atras","[0,0.707,-0.707]"))

direcciones.append(Direction("atras_izquierda","[0.707,0.707,0]"))
direcciones.append(Direction("izquierda","[1,0,0]"))
direcciones.append(Direction("frente_izquierda","[0.707,-0.707,0]"))
direcciones.append(Direction("frente","[0,-1,0]"))
direcciones.append(Direction("frente_derecha","[-0.707,-0.707,0]"))
direcciones.append(Direction("derecha","[-1,0,0]"))
direcciones.append(Direction("atras_derecha","[-0.707,0.707,0]"))
direcciones.append(Direction("atras","[0,1,0]"))

direcciones.append(Direction("arriba_atras_izquierda","[0.577,0.577,0.577]"))
direcciones.append(Direction("arriba_izquierda","[0.707,0,0.707]"))
direcciones.append(Direction("arriba_frente_izquierda","[0.577,-0.577,0.577]"))
direcciones.append(Direction("arriba_frente","[0,-0.707,0.707]"))
direcciones.append(Direction("arriba_frente_derecha","[-0.577,-0.577,0.577]"))
direcciones.append(Direction("arriba_derecha","[-0.707,0,0.707]"))
direcciones.append(Direction("arriba_atras_derecha","[-0.577,0.577,0.577]"))
direcciones.append(Direction("arriba_atras","[0,0.707,0.707]"))

direcciones.append(Direction("arriba","[0,0,1]"))



archi=open("pos.txt","w")
for i in range(0,len(direcciones)): 
    archi.write("==============================" + direcciones[i].n + "==============================")
    archi.write('\n')
    for j in range(0,len(direcciones)):
        archi.write(direcciones[i].n + '\t' + direcciones[j].n + '\t' + direcciones[i].d + '\t' + direcciones[j].d)
        archi.write('\n')
        
        
    
#print(str(vectorUnitarioVector([0.707,-0.707,-0.707])))
#BrazoSegunVectores([0,0,1] ,[0,0,1])
