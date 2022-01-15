# -*- coding: utf-8 -*-
#coding=utf-8
import xdrlib ,sys
import xlrd
import codecs
import os.path
import os
import math
import os

reload(sys)  
sys.setdefaultencoding('utf-8') 

exportCsvDir = os.path.abspath("./ExportCSV");
toEditorDir = os.path.abspath("../Assets/Editor/CSV/");
toEditorAes256keyFile = toEditorDir + "/" + "aes256.txt"
# aes256加密
aes256keyFile = "./aes256.key";

def UpdateSVNversion(svnVersion):
    

    #读取aeskey
    f_aes256  = codecs.open(aes256keyFile, "r", "utf-8")
    aes256key_list = f_aes256.readlines();
    f_aes256.close();
    aes256key = aes256key_list[0]
    print("秘钥:"+aes256key)
    print("版本号:"+str(svnVersion))

    #更新aesKey
    t_one_ase256 = codecs.open(aes256keyFile,"w","utf-8")
    t_one_ase256.write(aes256key);
    #t_one_ase256.write("\r\n");
    t_one_ase256.write(str(svnVersion));
    t_one_ase256.close();
    #写asekey到editor
    t_two_aes256 = codecs.open(toEditorAes256keyFile,"w","utf-8")
    t_two_aes256.write(aes256key);
    #t_two_aes256.write("\r\n");
    t_two_aes256.write(str(svnVersion));
    t_two_aes256.close();


#CSV拷贝到Editor目录
def CopyToEditor(fileName,suffixName,svnVersion):
    
    toEditorCSVversionDir = toEditorDir + "/" + str(svnVersion)+"/";

    if os.path.isdir(toEditorCSVversionDir) == False:
        os.makedirs(toEditorCSVversionDir)

    #读取csv文件 
    from_file_path = exportCsvDir+"/"+fileName+suffixName;
    f = codecs.open(from_file_path,"r","utf-8_sig")
    from_str = f.read();
    f.close();
    
    # 写入csv文件到editor
    if os.path.isdir(toEditorCSVversionDir) == False:
        os.makedirs(toEditorCSVversionDir)

    toFile = toEditorCSVversionDir+fileName+".csv";
    t = codecs.open(toFile, "w", "utf-8_sig")
    t.write(from_str);#将数据写入文件中
    t.close();#关闭文件流
    print("拷贝成功:"+toFile)


def getFileVersion():
    l = svn.local.LocalClient(exportCsvDir)
    for e in l.log_default():
        #print(e.revision)
        return e.revision 


def main():
    try:
        #正式模式打开
        svnVersion = 1
        print("船新的svnVersion:"+str(svnVersion))
        #更新svn version
        UpdateSVNversion(svnVersion)

        #拷贝csv
        list = os.listdir(exportCsvDir)
        for line in list:
            if os.path.splitext(line)[1] == ".csv":
                #print("加密文件："+line)
                CopyToEditor(os.path.splitext(line)[0],".csv",svnVersion);

        os.system("pause")
    except Exception as result:
        print(result)
        os.system("pause")
if __name__=="__main__":
    main();