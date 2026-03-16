# Rapport: Implementatie en training van een ML-agent in Unity

## Inleiding

Dit rapport toont de configuratie, implementatie en trainingsproces van een agent.
Het hoofddoel is is het realiseren van navigatie waarbij de agent een opdracht uitvoert uit meerdere fases:

- Het zoeken van een object.
- Verplaatsen naar een vastgelegde eindzone.

De opdracht is bedoeld om kennis te verbreden over reinforcement learning en navigatie via Ray Percepters en dit aan de docent te laten zien.

## Methoden

Voor het maken van het model is gebruikgemaakt van de Unity engine en de ML-agents toolkit ook van Unity.

1. Behaviour parameters & Agent
    - De parameter Continious Actions is 2, dit zorgt voor de besturing over de horizontale en verticale assen
    - Door middel van een Ray Perception Sensor 3D, met de nodige configuratie (detectable tags), kan de agent zien wat er rond zich staat.
    - De agent heeft een Rigidbody, met de Freeze Rotation op de X- en Z-as, dit zorgt voor het garanderen van een positionele stabiliteit

2. Override methods
    - `OnEpisodeBegin()`: Positioneert de agent terug naar de startlocatie na een val en plaatst het doelobject op een random locatie
    - `CollectObservation()`: Beperkt zich tot het doorgeven van de status die aangeeft of de eerste fase is voltooid.
    - `OnActionReceived()`: Regelt de bewegingsvectoren en de beloningen
      Per iteratie is er een aanhoudende straf van (-1f / maxSteps)
      Belongingen bestaan uit 3 stappen:
        - Richting het doelobject gaan geeft (0.001f \* (1.0f / distanceToTarget))
        - Het bereiken van het doelobject geeft een beloning van +0.5f
        - Wanner de agent op de GreenZone komt krijgt hij een beloning van +1f
          Het vallen van het platform geeft -1f
    - `Heuristic()`: Maakt het mogelijk om handmatig te testen

## Resultaten

Naarmate het vele testen en de nodige aanpassingen te doen was het voor de agent mogelijk om consistent de opdrachten uit te voeren.

In het begin was er weinig beweging, hierdoor leerde hij ook niet goed dat hij niet van het platform af mocht vallen.
De oplossing hiervoor was om de agent te straffen hoelang dat die erover doet om het doelobject op te pakken en daarna naar de GreenZone te bewegen

## Conclusie

Uit de resultaten werd duidelijk dat het succesvol training van de agent sterk afhankelijk is van een goed ontworpen beloningstructuur
In de eerste fase waren er alleen beloningen bij het oppakken van het doelobject en het bereiken van de GreenZone, hierdoor leerde de agent dat bewegen kan leiden tot een val van het platform, dit zorgde voor passiviteit

De succesvolle van de taak werd gerealiseerd door twee aanpassingen in de logica:

- Existentiële straf: De introductie van een continue, minimale straf per iteratie (-1f / MaxStep) zorgde voor een noodzaak voor efficiëntie, waardoor de agent werd aangezet om de acties zo snel mogelijk te voltooien en passiviteit werd afgeleerd.
- Proximity reward: Door een beloning toe te voegen gebaseerd op de afstand tot het doelobject (0.001f \* (1.0f / distanceToTarget)), ontving de agent directe feedback over de juiste bewegingsrichting.
