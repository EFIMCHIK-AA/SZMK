package com.example.szmkmobile;

import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

public class MainActivity extends AppCompatActivity {

    //Объявляем компоненты
    private Button Settings, ScanDoc, Exit,ScanBlank;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        //Присваиваем компоненты
        Settings = (Button)findViewById(R.id.Save_B);
        ScanDoc = (Button)findViewById(R.id.ScanDoc_B);
        ScanBlank = (Button)findViewById(R.id.ScanBlank_B);
        Exit = (Button)findViewById(R.id.Exit_B);

        Exit.setOnClickListener(
                new View.OnClickListener(){
                    @Override
                    public void onClick(View v){

                        AlertDialog.Builder MessageBox = new AlertDialog.Builder(MainActivity.this);
                        MessageBox.setMessage("Вы действительно хотите выйти?")
                                .setCancelable(false)
                                .setPositiveButton("Да", new DialogInterface.OnClickListener(){
                                    @Override
                                    public void onClick(DialogInterface dialog, int which){
                                        finish();
                                    }
                                })
                                .setNegativeButton("Нет", new DialogInterface.OnClickListener(){
                                    @Override
                                    public void onClick(DialogInterface dialog, int which){
                                        dialog.cancel();
                                    }
                                });
                        AlertDialog alert = MessageBox.create();
                        alert.setTitle("Внимание");
                        alert.show();
                    }
                }
        );

        Settings.setOnClickListener(
                new View.OnClickListener(){
                    @Override
                    public void onClick(View v){
                        startActivity( new Intent(MainActivity.this,SettingsActivity.class));
                    }
                }
        );

        ScanDoc.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                ScanerActivity.ScanDoc = true;
                ScanerActivity.Settings = false;
                ScanerActivity.ScanBlank = false;
                startActivity( new Intent(getApplicationContext(),ScanerActivity.class));
            }
        });

        ScanBlank.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                ScanerActivity.ScanDoc = false;
                ScanerActivity.Settings = false;
                ScanerActivity.ScanBlank = true;
                startActivity( new Intent(getApplicationContext(),ScanerActivity.class));
            }
        });
    }
}
