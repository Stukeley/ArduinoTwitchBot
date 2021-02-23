// ArduinoTwitchBot
// Sample code required to make the bot work on the Arduino side - this is just as example, you can use the incoming signals however you want!
// Schematic provided in respective repo.
// For more info, visit: https://github.com/Stukeley/ArduinoTwitchBot

// Pins.
int testPin = 8;
int followPin = 9;
int subPin = 10;
int hostPin = 11;

// Alert mesages (note - they can also be of type int or byte).
// IMPORTANT: these messages here must match the messages in ArduinoTwitchBot app.
String testMessage = "Test";
String followMessage = "Follow";
String subMessage = "Sub";
String hostMessage = "Host";

void setup() {
  // Setup pins.
  pinMode(testPin, OUTPUT);
  pinMode(followPin, OUTPUT);
  pinMode(subPin, OUTPUT);
  pinMode(hostPin, OUTPUT);

  // Setup Serial.
  Serial.begin(9600);
}

void loop() {
  // Message that is received.
  String message = "";

  // If we can read from Serial.
  if (Serial.available() > 0)
  {
    message = Serial.readString();

    // Display the message.
    Serial.println("Message received: " + message);

    // Turn the respective LED on based on what the content of the message was.
    
    if (message == testMessage)
    {
      // This is a test!
      digitalWrite(testPin, HIGH);
      delay(5000);
      digitalWrite(testPin, LOW);
    }
    else if (message == followMessage)
    {
      // A new follower!
      digitalWrite(followPin, HIGH);
      delay(5000);
      digitalWrite(followPin, LOW);
    }
    else if (message == subMessage)
    {
      // A new sub!
      digitalWrite(subPin, HIGH);
      delay(5000);
      digitalWrite(followPin, LOW);
    }
    else if (message == hostMessage)
    {
      // Somebody hosted the channel!
      digitalWrite(hostPin, HIGH);
      delay(5000);
      digitalWrite(followPin, LOW);
    }
    else
    {
      // Some signal that was not set.
      Serial.println("The received message was not configured");
    }
    
    message = "";
  }
}
