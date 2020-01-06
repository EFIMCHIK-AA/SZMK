package com.example.szmkmobile;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;

public class SettingsActivity extends AppCompatActivity {

    public static EditText IP, Port;
    public Button Scan, Save, Cancel;
    public final static String fileName= "param";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_settings);

        //Нахожим кнопки
        IP = (EditText)findViewById(R.id.IP_ET);
        Port = (EditText) findViewById(R.id.PORT_ET);
        Scan = (Button) findViewById(R.id.ScanDoc_B);
        Save = (Button) findViewById(R.id.Save_B);
        Cancel = (Button) findViewById(R.id.Cancel_B);

        String Params = Read(fileName);
        if(Params != null)
        {
            if(Params.length() != 1)
            {
                String [] Temp = Params.split("_");
                IP.setText(Temp[0]);
                Port.setText(Temp[1]);
            }
            else
            {
                IP.setText("000.000.000.000");
                Port.setText("0000");
            }
        }

        Save.setOnClickListener(
                new View.OnClickListener(){
                    @Override
                    public void onClick(View v){
                        try {
                            Write(fileName,IP.getText().toString(),Port.getText().toString());
                            Log.d("LOG","Файл параметров записан");
                            finish();
                        } catch (FileNotFoundException e) {
                            e.printStackTrace();
                        }
                    }
                }
        );



        Scan.setOnClickListener(
                new View.OnClickListener(){
                    @Override
                    public void onClick(View v){
                        ScanerActivity.Settings = true;
                        ScanerActivity.ScanBlank = false;
                        ScanerActivity.ScanDoc = false;
                        startActivity( new Intent(getApplicationContext(),ScanerActivity.class));
                    }
                }
        );

        Cancel.setOnClickListener(
                new View.OnClickListener(){
                    @Override
                    public void onClick(View v){
                        finish();
                    }
                }
        );
    }

    public void Write(String FileName,String IP, String Port) throws FileNotFoundException {
        try
        {
            String IPwr = IP;
            String PortWr = Port;

            if(IP == "" || Port == "")
            {
                IPwr = "000.000.000.000";
                PortWr = "0000";
            }

            String DataTemp = IPwr + PortWr;

            if(DataTemp.indexOf("_") != -1)
            {
                IPwr = "000.000.000.000";
                PortWr = "0000";
            }

            String Data = IPwr + "_" + PortWr;
            BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(openFileOutput(FileName,MODE_PRIVATE)));
            writer.write(Data);
            writer.close();
            Toast.makeText(SettingsActivity.this, "Параметры успешно сохранены", Toast.LENGTH_LONG).show();
        }
        catch (FileNotFoundException e)
        {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public String Read(String fileName)
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
}
