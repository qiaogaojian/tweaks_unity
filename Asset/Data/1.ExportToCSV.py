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

#打开excel文件
def open_excel(file):
        data = xlrd.open_workbook(file)
        return data


#导出excel表格到csv
def ExcelToCSV(fileName, suffixName):
    filePath = fileName+suffixName

    data = open_excel(filePath)
    table = data.sheets()[0] # 只导出第一个sheet的数据
    nrows = table.nrows #行数
    ncols = table.ncols #列数
    csvpath = "ExportCSV/"+fileName+".csv"
    f = codecs.open(csvpath, "w", "utf-8_sig")
    print("success!："+csvpath)
    ignoreDic = {-1:-1};
    for rows in range(0,nrows):
        tempList = table.row_values(rows);
        for cols in range(0,ncols):
            strdata = str(tempList[cols]);
            # 是否包含/ignore
            #print(strdata.find("/ignore"))
            if rows == 0 and strdata.find("/ignore") != -1 :
                print("是/ignore跳过该列："+str(cols)+"colum存入字典");
                ignoreDic[cols] = cols;

            #字典存在就跳过
            if ignoreDic.__contains__(cols):
                #print("跳过");
                continue;

            #排除策略
            # 1 去掉英文逗号
            strdata = strdata.replace(',','，')
            # 2 强转Float为float
            if(strdata == "Float"):
                strdata = "float"
            # 3 int自动变为float的问题


            if strdata.replace('.','',1).isdigit() and float(strdata)  % 1 == 0: #checking for the integer:
                print(strdata+"是int型")
                strdata=str(int(tempList[cols]))      #solving your problem and printing the integer
                #print("转换后"+str(strdata))
            else:
                strdata
            ##################
            #print(str(ncols) + "...." + str(cols) + "....." + strdata)
            f.write(strdata);#将数据写入文件中
            if cols < ncols-1:
                 f.write(','); #每个数据之间使用tab键分隔，不使用逗号分隔！
        f.write("\r\n");#每行数据写完后使用换行符\r\n作为结尾
    f.close();#关闭文件流


def main():
    try:
        rootdir = './'   # 需要遍历的文件夹，这里设定为当前文件夹
        list = os.listdir(rootdir)
        #测试
        #ExcelToCSV("./ConsumeData",".xlsx")
        #return
        for line in list:
            #filepath = os.path.join(rootdir, line)
            if os.path.isfile(line) and (os.path.splitext(line)[1] == ".xlsx"):
                #print(line+"后缀名 "+os.path.splitext(line)[1])
                if line.find('~$')!= -1:
                    continue
                print("ready:" + line)
                ExcelToCSV(os.path.splitext(line)[0],".xlsx"); #生成csv文件


        print("over....")
        os.system("pause")

    except Exception as result:
        print(result)
        os.system("pause")
if __name__=="__main__":
    main();