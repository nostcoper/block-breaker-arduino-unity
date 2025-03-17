#include <Arduino.h>

const int ledRojo = 11;
const int ledVerde = 10;
const int ledAmarillo = 9;
const int pulsador = 7;
// Pin del potenciómetro
const int potPin = A0;

// Intervalo para enviar el valor del potenciómetro
unsigned long lastPrintTime = 0;
const unsigned long printInterval = 50; // cada 50 ms

// Variable para almacenar la cantidad de vidas actual
int lives = 3;

// Función para actualizar los LEDs según el número de vidas
void updateLEDs(int lifeCount) {
  if(lifeCount >= 3) {
    digitalWrite(ledRojo, HIGH);
    digitalWrite(ledVerde, HIGH);
    digitalWrite(ledAmarillo, HIGH);
  } else if(lifeCount == 2) {
    digitalWrite(ledRojo, LOW);
    digitalWrite(ledVerde, HIGH);
    digitalWrite(ledAmarillo, HIGH);
  } else if(lifeCount == 1) {
    digitalWrite(ledRojo, LOW);
    digitalWrite(ledVerde, HIGH);
    digitalWrite(ledAmarillo, LOW);
  } else {
    digitalWrite(ledRojo, LOW);
    digitalWrite(ledVerde, LOW);
    digitalWrite(ledAmarillo, LOW);
  }
}

void setup() {
  Serial.begin(9600);
  
  // Configura los pines de los LEDs como salidas
  pinMode(ledRojo, OUTPUT);
  pinMode(ledVerde, OUTPUT);
  pinMode(ledAmarillo, OUTPUT);
  pinMode(pulsador, INPUT);
  
  // Enciende todos los LEDs al inicio (3 vidas)
  digitalWrite(ledRojo, HIGH);
  digitalWrite(ledVerde, HIGH);
  digitalWrite(ledAmarillo, HIGH);


}

void loop() {
  // Procesa el mensaje entrante para actualizar las vidas y los LEDs
  if (Serial.available() > 0) {
    String input = Serial.readStringUntil('\n');
    if (input.startsWith("L:")) {
      int newLives = input.substring(2).toInt();
      lives = newLives;
      Serial.println(lives);
      updateLEDs(lives);
    }
  }
  
  // Envía periódicamente el valor del potenciómetro
  if (millis() - lastPrintTime >= printInterval) {
    int potValue = analogRead(potPin);
    Serial.print("P:");
    Serial.println(potValue);
    lastPrintTime = millis();
  }

  if (digitalRead(pulsador) == 1){
      Serial.print("Launch:");
      Serial.println("true");
  }else{
    
  }
}