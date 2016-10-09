package com.neutralspacestudios.brs2;

import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.net.Uri;
import android.os.Build;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.TextView;
import android.os.StrictMode;
import android.graphics.drawable.Drawable;
import android.widget.Toast;

import com.google.android.gms.appindexing.Action;
import com.google.android.gms.appindexing.AppIndex;
import com.google.android.gms.appindexing.Thing;
import com.google.android.gms.common.api.GoogleApiClient;

import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;

///*** Mickey McCargish, 10/2016 ***///
public class Main extends AppCompatActivity implements SensorEventListener {
    int id = 0;
    int idcount = 0;
    int red = 0;
    int green = 0;
    int blue = 0;
    int painting = 0;
    String IP = "10.18.1.153";
    TextView player;
    TextView colorDiplay;
    Button paintb;
    EditText IPbox;
    Button submitBox;
    ImageButton orangeb;
    ImageButton blackb;
    ImageButton sapgreenb;
    ImageButton alizcrimb;
    ImageButton vdbrownb;
    ImageButton dksiennab;
    ImageButton phthblueb;
    ImageButton phthgreenb;
    ImageButton calyelb;
    ImageButton redb;
    ImageButton yellochre;
    ImageButton white;
    Button resetb;
    /**
     * ATTENTION: This was auto-generated to implement the App Indexing API.
     * See https://g.co/AppIndexing/AndroidStudio for more information.
     */
    private GoogleApiClient client;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        //set up notitle
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        //set up full screen
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN,
                WindowManager.LayoutParams.FLAG_FULLSCREEN);

        setContentView(R.layout.activity_main);
        if (Build.VERSION.SDK_INT > 9) {
            StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
            StrictMode.setThreadPolicy(policy);
        }
        sensorManager = (SensorManager) getSystemService(SENSOR_SERVICE);
        sensorManager.registerListener(this, sensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER), SensorManager.SENSOR_DELAY_NORMAL);
        IPbox = (EditText) findViewById(R.id.editText);
        submitBox = (Button) findViewById(R.id.submitIP);
        paintb = (Button) findViewById(R.id.p);
        player = (TextView) findViewById(R.id.player);
        colorDiplay = (TextView) findViewById(R.id.paint);
        colorDiplay.setText("MIDNIGHT BLACK");
        orangeb = (ImageButton) findViewById(R.id.orangebutton);
        blackb = (ImageButton) findViewById(R.id.blackbutton);
        phthgreenb = (ImageButton) findViewById(R.id.phthalogreenb);
        phthblueb = (ImageButton) findViewById(R.id.phthblueb);
        white = (ImageButton) findViewById(R.id.white);
        alizcrimb = (ImageButton) findViewById(R.id.azicrim);
        calyelb = (ImageButton) findViewById(R.id.calciumyellow);
        dksiennab = (ImageButton) findViewById(R.id.darksienna);
        redb = (ImageButton) findViewById(R.id.redbutton);
        vdbrownb = (ImageButton) findViewById(R.id.vdbrown);
        sapgreenb = (ImageButton) findViewById(R.id.sapgreen);
        yellochre = (ImageButton) findViewById(R.id.yellochre);
        resetb = (Button) findViewById(R.id.reset);
        addTextListener(player);
        addListenerOnButton(paintb);
        addSubmitIP(submitBox);
        addListenerOnButtonColor(orangeb, 0, 47, 255,"INDIAN YELLOW");
        addListenerOnButtonColor(blackb, 255, 255, 255,"MIDNIGHT BLACK");
        addListenerOnButtonColor(sapgreenb, 245, 215, 241,"SAP GREEN");
        addListenerOnButtonColor(alizcrimb, 177, 234, 255,"ALIZARIN CRIMSON");
        addListenerOnButtonColor(vdbrownb, 221, 228, 234,"VAN DYKE BROWN");
        addListenerOnButtonColor(dksiennab, 160, 209, 224,"DARK SIENNA");
        addListenerOnButtonColor(phthblueb, 242, 255, 191,"PHTHALO BLUE");
        addListenerOnButtonColor(phthgreenb, 249, 209, 194,"PHTHALO GREEN");
        addListenerOnButtonColor(calyelb, 0, 19, 255,"CADMIUM YELLOW");
        addListenerOnButtonColor(redb, 36, 255, 255,"BRIGHT RED");
        addListenerOnButtonColor(yellochre, 0, 150, 255,"YELLOW OCHRE");
        addListenerOnButtonColor(white, 0, 0, 0,"TITANIUM WHITE");
        addResetButton(resetb);
        // ATTENTION: This was auto-generated to implement the App Indexing API.
        // See https://g.co/AppIndexing/AndroidStudio for more information.
        client = new GoogleApiClient.Builder(this).addApi(AppIndex.API).build();
    }

    private void addSubmitIP(Button submitBox) {
        submitBox.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View arg0) {
                if(validIP(IPbox.getText().toString()))
                    IP = IPbox.getText().toString();
            }
        });
    }

    private SensorManager sensorManager;
    double px = 0, py = 0, pz = 0;   // these are the acceleration in x,y and z axis
    double vx = 0, vy = 0, vz = 0;
    double ax = 0, ay = 0, az = 0;
    double dt = System.nanoTime(), odt = System.nanoTime(), diff = 0;
    boolean setgrav = true;

    @Override
    public void onAccuracyChanged(Sensor arg0, int arg1) {
    }

    @Override
    public void onSensorChanged(SensorEvent event) {
        if (event.sensor.getType() == Sensor.TYPE_ACCELEROMETER) {
            odt = dt;
            dt = System.nanoTime();
            diff = (dt - odt) / 1000000000000.0;
            if (setgrav) {
                ax = event.values[0];
                ay = event.values[1];
                az = event.values[2];
                vx = 0;
                vy = 0;
                vz = 0;
                px = 0;
                py = 0;
                pz = 0;
                setgrav = false;
            }
            if (Math.abs(event.values[0]) > 1.0) {
                vx += (event.values[0] - ax) * diff;
                px += vx;
            } else {
                if (Math.abs(vx) < .08)
                    vx = 0;
                if (vx < 0)
                    vx -= .08;
                if (vx > 0)
                    vx += .08;
            }
            if (Math.abs(event.values[1]) > 1.0) {
                vy += (event.values[1] - ay) * diff;
                py += vy;
            } else {
                if (Math.abs(vy) < .08)
                    vy = 0;
                if (vy > 0)
                    vy -= .08;
                if (vy < 0)
                    vy += .08;
            }
            data(px, py);
        }

    }

    public void addListenerOnButton(Button ib) {
        ib.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View arg0) {
                if (painting >= 1) {
                    painting = 0;
                    Toast.makeText(getApplicationContext(), "Paint mode OFF.",
                            Toast.LENGTH_SHORT).show();
                } else {
                    painting = 1;
                    Toast.makeText(getApplicationContext(), "Paint mode ON",
                            Toast.LENGTH_SHORT).show();
                }
            }
        });
    }

    public void addTextListener(TextView tv) {
        tv.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View arg0) {
                if(id <= 2){
                    id ++;
                }else{
                    id = 0;
                }
                player.setText(""+(id+1));
            }
        });
    }

    public void addResetButton(Button ib) {
        ib.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View arg0) {
                px = 0;
                py = 0;
                pz = 0;
                vx = 0;
                vy = 0;
                vz = 0;
                ax = 0;
                ay = 0;
                az = 0;
                painting = 0;
                Toast.makeText(getApplicationContext(), "Paintbrush is now reset.",
                        Toast.LENGTH_SHORT).show();
            }
        });
    }

    public void addListenerOnButtonColor(ImageButton ib, final int r, final int g, final int b, final String str) {
        ib.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View arg0) {
                red = r;
                green = g;
                blue = b;
                colorDiplay.setText(str);
            }
        });

    }

    public void data(double pX, double pY) {
        final ByteArrayOutputStream baos = new ByteArrayOutputStream();
        final DataOutputStream daos = new DataOutputStream(baos);
        try {
            daos.writeInt(id);
            daos.writeDouble(pX);
            daos.writeDouble(pY);
            daos.writeInt(red);
            daos.writeInt(green);
            daos.writeInt(blue);
            daos.writeInt(painting);
            daos.close();
            final byte[] bytes = baos.toByteArray();

            DatagramSocket clientSocket = new DatagramSocket();
            InetAddress IPAddress = InetAddress.getByName(IP);
            DatagramPacket sendPacket = new DatagramPacket(bytes, bytes.length, IPAddress, 13370);
            clientSocket.send(sendPacket);
            clientSocket.close();

        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    /**
     * ATTENTION: This was auto-generated to implement the App Indexing API.
     * See https://g.co/AppIndexing/AndroidStudio for more information.
     */
    public Action getIndexApiAction() {
        Thing object = new Thing.Builder()
                .setName("Main Page") // TODO: Define a title for the content shown.
                // TODO: Make sure this auto-generated URL is correct.
                .setUrl(Uri.parse("http://[ENTER-YOUR-URL-HERE]"))
                .build();
        return new Action.Builder(Action.TYPE_VIEW)
                .setObject(object)
                .setActionStatus(Action.STATUS_TYPE_COMPLETED)
                .build();
    }

    @Override
    public void onStart() {
        super.onStart();

        // ATTENTION: This was auto-generated to implement the App Indexing API.
        // See https://g.co/AppIndexing/AndroidStudio for more information.
        client.connect();
        AppIndex.AppIndexApi.start(client, getIndexApiAction());
    }

    @Override
    public void onStop() {
        super.onStop();

        // ATTENTION: This was auto-generated to implement the App Indexing API.
        // See https://g.co/AppIndexing/AndroidStudio for more information.
        AppIndex.AppIndexApi.end(client, getIndexApiAction());
        client.disconnect();
    }

    public static boolean validIP (String ip) {
        try {
            if ( ip == null || ip.isEmpty() ) {
                return false;
            }

            String[] parts = ip.split( "\\." );
            if ( parts.length != 4 ) {
                return false;
            }

            for ( String s : parts ) {
                int i = Integer.parseInt( s );
                if ( (i < 0) || (i > 255) ) {
                    return false;
                }
            }
            if ( ip.endsWith(".") ) {
                return false;
            }

            return true;
        } catch (NumberFormatException nfe) {
            return false;
        }
    }
}
