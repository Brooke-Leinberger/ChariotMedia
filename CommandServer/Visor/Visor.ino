#include <Servo.h>

#define YAW_PIN 6
#define PIT_PIN 5
#define PIT_ZRO 90
#define PIT_ZRO 90

Servo yaw;
Servo pitch;

byte buffer[260], header[5], dustpan[64];

void setup()
{
    pitch.attach(PIT_PIN);
    yaw.attach(YAW_PIN);
    Serial.begin(9600, SERIAL_8O1);
    Serial.write("Beginning...");
}

void execute_buffer()
{
    pitch.write((int) buffer[0]);
    yaw.write((int) buffer[1]);
    delay(15);
    display_buffer();
}

void empty_buffer()
{
  Serial.readBytes(dustpan, 64);
}

void display_buffer()
{
  Serial.print("255,");
  for(int i = 0; i < 4; i++)
  {
      Serial.print(header[i]);
      Serial.print(",");
  }
  for(int i = 0; i < header[2]; i++)
  {
      Serial.print(buffer[i]);
      Serial.print(",");
  }
  Serial.print("\n"); 
}

void loop()
{
    //Serial.println(Serial.available());
    
    if(Serial.available() < 6)
        return;

    int bite = Serial.read();

    //wait for start
    if(bite != 255)
        return;

    //read header
    Serial.readBytes(header, 5);

    //check parity
    if(header[3] != header[0] + header[1] + header[2])
        return;

    //check if buffer is full
    if(Serial.available() > 60)
    {
        empty_buffer();
        return;
    }

    //if everything is good, read buffer
    while(Serial.available() < header[2]);
    Serial.readBytes(buffer, header[2]);

    //check data parity
    int parity = 0;
    for(int i = 0; i < header[2]; i++)
        parity += (int)buffer[i];

    //display_buffer();
    if(header[4] != parity % 255)
        return;

    //after double and triple checks, execute
    execute_buffer();
}
