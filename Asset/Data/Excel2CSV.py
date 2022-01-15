# -*- coding: utf-8 -*-
# coding=utf-8
import xlrd
import codecs
import shutil
import os.path
import os


relaPath = os.path.split(os.path.realpath(__file__))[0]
excelPath = relaPath + '/Excel/'
csv1Path = relaPath + '/Csv/'
csv2Path = relaPath + '/../../Assets/StreamingAssets/Csv/'


# 打开excel文件
def openExcel(file):
    data = xlrd.open_workbook(file)
    return data


# excel转csv
def excel2CSV(fullPath):
    excelData = openExcel(fullPath)
    table = excelData.sheets()[0]                       # 只导出第一个sheet的数据
    rowsNum = table.nrows                               # 行数
    colsNum = table.ncols                               # 列数

    ignoreDic = {-1: -1}

    excelName = os.path.split(fullPath)[1].split('.')[0]
    outPath = csv1Path + excelName + ".csv"
    # The utf-8-sig codec will decode both utf-8-sig-encoded text and text encoded with the standard utf-8 encoding
    csvfile = codecs.open(outPath, "w", "utf-8_sig")

    for rows in range(0, rowsNum):
        temRow = table.row_values(rows)

        for colIndex in range(0, colsNum):
            temCol = str(temRow[colIndex])

            if rows == 0 and temCol.startswith("//"):   # Excel表中第一行以"//"开头的列忽略
                ignoreDic[colIndex] = colIndex
            if ignoreDic.__contains__(colIndex):
                continue

            # 1.去掉英文逗号
            temCol = temCol.replace(',', '，')
            # 2.强转Float为float
            if(temCol == "Float"):
                temCol = "float"
            # 3.int自动变为float的问题
            if temCol.replace('.', '', 1).isdigit() and float(temCol) % 1 == 0:
                temCol = str(int(temRow[colIndex]))

            csvfile.write(temCol)
            if colIndex < colsNum-1:
                # 每个数据之间使用逗号分隔，不使用tab键分隔
                csvfile.write(',')
        csvfile.write("\r\n")                           # 每行数据写完后添加换行符: \r\n
    csvfile.close()


# 复制csv到SteamingAssets文件夹
def copyCsv():
    shutil.rmtree(csv2Path)
    shutil.copytree(csv1Path, csv2Path)

# 递归查找文件
def recurDir(rootPath):
    listT = os.listdir(rootPath)                    # 列出文件夹下所有的目录与文件
    for i in range(0, len(listT)):
        path = os.path.join(rootPath, listT[i])
        if os.path.isfile(path):                    # 是文件，进入文件，查找
            excel2CSV(path)
            # print(os.path.split(path)[0])         # 文件名前面的所有路径
            # print(os.path.split(path)[1])         # 不带路径的文件名和后缀
        else:                                       # 是文件夹，进入文件夹，递归
            recurDir(path)


def main():
    try:
        recurDir(excelPath)
        copyCsv()
    except Exception as result:
        print(result)
        os.system("pause")


if __name__ == "__main__":
    main()
