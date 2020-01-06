package com.example.szmkmobile;

public class ScanData {

    public static String Data = "";

    public static String[] GetParam(String Data)
    {
        String [] Temp = Data.split("_");

        if(Temp.length != 2)
        {
            Temp = new String[2];
            Temp[0] = "000.000.000.000";
            Temp[1] = "0000";
        }

        return  Temp;
    }
}
