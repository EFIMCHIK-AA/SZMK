package com.example.szmkmobile;

import java.net.InetAddress;
import java.net.Socket;
import java.io.*;

public class Client{
    public static void SendMessage(){
        Socket socket = null;
        try {
            socket = new Socket("192.168.1.105", 49000);
        } catch (IOException e) {
            e.printStackTrace();
        }

        OutputStream out = null;
        try {
            out = socket.getOutputStream();
        } catch (IOException e) {
            e.printStackTrace();
        }
        PrintWriter output = new PrintWriter(out);
        output.println("Hello from Android");
        try {
            socket.close();
        } catch (IOException e) {
            e.printStackTrace();
    }
    }
}