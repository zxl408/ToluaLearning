import os,sys

statinfo = os.stat("D:/UnityWorkSpace/TestWorkSpace/tolua-master/AssetBundles/Android/lua.unity3d".encode('utf-8'))
print(statinfo)
def buildAllRescource():
    print("start build ...")
    currentPath = os.path.abspath('.')
    listdir = os.listdir(currentPath)
    for dirName in listdir:        
        if(os.path.isdir(dirName)):            
            List = getFiles(dirName)   
            buildFiles(dirName,List)
       
def buildFiles(dirRoot,L):
    md5_List={}
    resourcelistName = os.path.join(dirRoot,"resourcelist.txt")
    fo = open(resourcelistName,'w')
    for relativePath in L:
        fullPath = os.path.join(dirRoot,relativePath)
        print("fullPath:"+fullPath)
        statinfo = os.stat(fullPath)
        md5str= fullPath+"&"+str(statinfo.st_mtime)+"&"+str(statinfo.st_size);         
        fo.write(relativePath+"\t"+md5_passwd(md5str)+"\t"+str(statinfo.st_size)+"\n") 
   
    
    
def getFiles(file_dir):
    L=[]
    def getAllFiles(file_dir):
       for root,dirs,files in os.walk(file_dir):
          for file in files:
              fileName = os.path.join(file_dir,file)
              L.append(fileName)
              print('file:...'+fileName)             
          for dirName in dirs:
              dirit=os.path.join(file_dir,dirName)
              print("dir:----"+dirit)
              getAllFiles(dirit)
            
          return
    getAllFiles(file_dir)
    Lnew=[]
    for li in L:
        if(li.find('.manifest')==-1):            
            Lnew.append(li[len(file_dir)+1:])
    return Lnew

         
    
privateKey='zxlpassword$#'
def md5_passwd(str):
      str = str+":"+privateKey     
      import hashlib
      md = hashlib.md5()
      md.update(str.encode())
      res = md.hexdigest()
      return res
buildAllRescource()
