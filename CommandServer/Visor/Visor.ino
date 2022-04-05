#include <SPI.h>
#include <Servo.h>

#define YAW_PIN 6
#define PIT_PIN 5
#define PIT_ZRO 90
#define PIT_ZRO 90

Servo yaw;
Servo pitch;

int length, ind;
bool ready;
byte buffer[260];

void setup()
{
    pitch.attach(PIT_PIN);
    yaw.attach(YAW_PIN);
    length = 4;
    ind = 0;
    ready = false;
}

ISR (SPI_STC_vect)
{
    byte c = SPDR;
    
    if(ready)
    {
        if (length != -1 && ind >= length)
            return;
          
        if (ind >= 4)
        {
            if(ind == 4)
                length = c;
            
            buffer [ind++] = c;
        }
        
        buffer [ind++] = c;
            
        if (ind + 1 == length)
            execute_buffer();
    }
    
    else if(c == 255)
    {
        ready = true;
        ind = 1;
    }
}

void execute_buffer()
{
    pitch.write((int) buffer[4]);
    yaw.write((int) buffer[5]);
    length = -1;
    ind = 0;
}

void loop()
{
    
}