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

# Rapport: Implementatie en Training van de Obelix ML-Agent

## Inleiding

Dit rapport beschrijft de ontwikkeling van de agent Obelix in Unity met behulp van de ML-Agents toolkit. Het doel is om de agent via Ray Perception Sensors 'Menhirs' te laten zoeken, op te pakken en terug te brengen naar een bestemming.

## Methoden

1. Spelomgeving en objecten
De omgeving bestaat uit een speelveld met 3 hoofdcomponenten.
- De agent Obelix: Een capsule met een rigidbody (geen rotatie op X- en Z-as) en 2 Ray Perception Sensors voor visuele waarnemingen.
- Menhirs: Objecten die willekeurig in de scene worden gegenereerd en die de agent moet verzamelen.
- Bestemmingen: Statische zones waar de menhirs moeten worden afgeleverd.

2. Obeservaties, Acties en Logica
De agent is gebouwd uit:
- Observaties: De agent maakt gebruik van handmatige observaties en sensors.
   - Boolean status of hij momenteel een menhir bezit.
   - Visuele data via de Ray Perception Sensors die de tags Menhir en Destination herkenner.
- Actie
    - Er wordt gebruikgemaakt van Discrete Actions met twee taken (voorwaarts/achterwaarts bewegen en links/rechts roteren).

3. Beloningen
- Positieve belongingen:
    - Het oppakken van een menhir wanner de agent nog geen heeft geeft +1.0f.
    - Het succesvol afleveren van een menhir bij de bestemming geeft +2.0f.
- Negatieve beloningen:
    - Er is een straf van -0.001f per stap zodat de agent niet rondjes blijft draaien.
    - Een kleine straf van -0.05f voor het aanraken van de bestemming zonder menhir.
    - Een straf van -1.0f wanneer de agent van de map valt.

## Resultaten
In de eerste fase was de agent enorm passief of hij liep van het platform af. Dit kwam door te weinig richting feedback

Door deze toevoegingen ging het beter
- Het verkleining van de spawn radius van de menhirs waardoor hij ze sneller zag
- Het verhogen van batch_size en buffer_size in de configuratie

Tijdens de uitbereiding naar meerdere Menhirs en Destinations mislukte de training. De agent wist niet meer wat hij moest doen en was gewoon rond aan het dwalen.
Volgens mij zou dit kunnen komen doordat er te veel informatie was en kreeg dit niet werkende.

## Conclusie
Na het training van de Agent was het weer duidelijk dat het kiezen van goede beloningen enorm belangrijk is.
Het toevoegen van de Ray Perception Sensors was niet eenvoudig en zorgde voor problemen.
Het kiezen van een goede configuratie is ook belangrijk
