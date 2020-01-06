package com.example.szmkmobile;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AppCompatActivity;

import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.widget.Toast;
import com.google.zxing.Result;
import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.io.UnsupportedEncodingException;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.net.SocketAddress;
import java.net.SocketException;

import me.dm7.barcodescanner.zxing.ZXingScannerView;

public class ScanerActivity extends AppCompatActivity implements ZXingScannerView.ResultHandler{

    ZXingScannerView ScannerView;
    static boolean Settings = false;
    static boolean ScanDoc = false;
    static boolean ScanBlank = false;
    public static String msg = "default";
    public static String server_address = "192.168.1.105";
    public static Integer server_port = 49000;
    public static Boolean Abort = false;
    public static LongOperation lo = null;
    public static Socket socket = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        ScannerView = new ZXingScannerView(this);
        setContentView(ScannerView);
    }

    //Хандлер возврата результата из сканера
    @Override
    public void handleResult(Result result) {
       ScanData.Data = result.getText();

       if(Settings)
       {
           if(ScanData.Data != "" && ScanData.Data != null)
           {
               String [] Temp = ScanData.GetParam(ScanData.Data);

               if(Temp.length == 2)
               {
                   SettingsActivity.IP.setText(Temp[0]);
                   SettingsActivity.Port.setText(Temp[1]);
               }
               else {
                   SettingsActivity.IP.setText("000.000.000.000");
                   SettingsActivity.Port.setText("0000");
               }
           }
       }
       else if(ScanDoc)
       {
           try
           {
               String str = new String(ScanData.Data.getBytes("ISO-8859-1"), "Cp1251");
               ScanData.Data = str;
           } catch (UnsupportedEncodingException e) {
               e.printStackTrace();
           }

           if (ScanData.Data.indexOf("\u001D") != -1)
           {
               ScanData.Data=ScanData.Data.replaceAll("\u001D","и");
           }

           int Count = ScanData.Data.split("_").length;

           if(Count == 6)
           {
               String [] ConnectParam = Read(SettingsActivity.fileName).split("_"); // 0 - IP 1- Port
               int Port = Integer.parseInt(ConnectParam[1]);
               msg=ScanData.Data;
               sendMessage();
           }
       }
       else if(ScanBlank)
       {
           try
           {
               String str = new String(ScanData.Data.getBytes("ISO-8859-1"), "Cp1251");
               ScanData.Data = str;
           } catch (UnsupportedEncodingException e) {
               e.printStackTrace();
           }

           if (ScanData.Data.indexOf("\u001D") != -1)
           {
               ScanData.Data=ScanData.Data.replaceAll("\u001D","и");
           }

           int Count = ScanData.Data.split("_").length;

           if(Count >= 3)
           {
               String [] ConnectParam = Read(SettingsActivity.fileName).split("_"); // 0 - IP 1- Port
               int Port = Integer.parseInt(ConnectParam[1]);
               msg=ScanData.Data;
               sendMessage();
           }
       }

       Toast.makeText(this, ScanData.Data, Toast.LENGTH_LONG).show();
       onBackPressed();
    }

    private String Read(String fileName)
    {
        try
        {
            BufferedReader reader = new BufferedReader(new InputStreamReader(openFileInput(fileName)));
            String line;

            while ((line = reader.readLine()) != null) {
                return line;
            }
        }
        catch (FileNotFoundException e)
        {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }

        return null;
    }

    @Override
    protected void onPause(){
        ScannerView.stopCamera();
        super.onPause();

    }

    @Override
    protected void onResume(){
        ScannerView.setResultHandler(this);
        ScannerView.startCamera();
        super.onResume();
    }
public class LongOperation extends AsyncTask<String, Void, String> {


    @RequiresApi(api = Build.VERSION_CODES.N)
    @Override
    protected String doInBackground(String... params) {

        socket = null;
        SocketAddress address = new InetSocketAddress(server_address, server_port);

        socket = new Socket();


        try {
            socket.connect(address, 3000);
        } catch (IOException e) {
            Log.d("time", "no worky X");
            e.printStackTrace();

        }
        try {
            socket.setSoTimeout(3000);
        } catch (SocketException e) {
            Log.d("timeout", "server took too long to respond");

            e.printStackTrace();
            return "Can't Connect";
        }
        OutputStream out = null;
        try {
            out = socket.getOutputStream();
        } catch (IOException e) {
            e.printStackTrace();
        }
        PrintWriter output = new PrintWriter(out);


        output.print(msg);
        //output.flush();

//read
        String str = "waiting";
        BufferedReader br = null;
        try {
            br = new BufferedReader(new InputStreamReader(socket.getInputStream()));
        } catch (IOException e) {
            e.printStackTrace();
        }
        try {
            Log.d("test", "trying to read from server");

            String line;
            str = "";
            while ((line = br.readLine()) != null) {
                Log.d("read line", line);
                str = str + line;
                str = str + "\r\n";
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
        if (str != null) {
            Log.d("test", "trying to print what was just read");
            System.out.println(str);
        }


//read
        output.close();

//read
        try {
            br.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
//read
        try {
            socket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }

        Log.d("tag", "done server");
        return str;
    }


    protected void onCancelled() {
        Log.d("cancel", "ca");
        try {
            socket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
        Abort = false;
    }

}
    //Метод отправки данных на сервер
    public void sendMessage() {
        if (Abort == true) {
            lo.cancel(false);
            Log.d("Aborting", Abort.toString());
        } else {
            lo = new LongOperation();
            lo.execute();
        }
        Abort = true;
    }

}
