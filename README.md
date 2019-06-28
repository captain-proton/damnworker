# Dependency Injection, Events, Delegates und Threads

Die meiste Dokumentation des Codes findet sich im Code in den Klassen `Program.cs`, `DamnModule.cs`, `Boss.cs` und `Worker.cs`. Da hier direkt eine Vielzahl an unterschiedlichen  Technologien vermischt werden ist es ggfs. schwierig einen Überblick zu bekommen oder den richtigen Startpunkt zu finden.

## Dependency Injection

Wir wollen in unserem Programm eine lose Kopplung von Komponenten, damit diese einfach austauschbar, erweiterbar etc. sind.

Beispiel für harte Kopplung:

```cs
var someList = new List<int>();
```

Mit dem letzten Snippet wird eine direkte Abhängigkeit zur Klasse List erstellt. Jetzt werden Ihr wahrscheinlich sagen ... "ja die Klasse List wird sich doch vermutlich nie ändern" ... aber hier gehts nicht um `List`, sondern um sowas wie `MyFancyInterface`. Dieser wird sich durchaus ändern. Zu diesem Zweck haben sich unterschiedliche Leute tolle Mechanismen ausgedacht wie sowas auszusehen hat und sind auf Dependency Injection gekommen. In diesem Beispielprojekt wird der Dependency Injektor `Ninject` verwendet. Startpunkt zum Lesen ist die Datei `Program.cs`.

## Events und Delegates

Ein Super Video zur Erklärung gibts natürlich bei [YouTube](https://www.youtube.com/watch?v=jQgwEsJISy0). Es handelt sich hier um die Umsetzung des Observer Patterns. Instanz A ist interessiert an einem Ereignis, welches Instanz B erzeugt und alle Interessenten benachrichtigt. Für die Beispielanwendung ist der Boss der Subscriber und die Worker die Publisher. Wenn du das Video gesehen hast (schau es spätestens jetzt an!!!) geht die Reise in der Klasse `Worker.cs` weiter.

## Threads (Process vs Thread vs Task)

Hier gibts unglaublich viel nichts aussagende Doku in .NET, aber zum Glück auch in stackoverflow. Um direkt zum Punkt zu kommen, warum hier Threads verwendet werden ... Prozesse teilen keine Resourcen im Programm, so dass Kommunikation schwierig wird. Tasks sind dazu gedacht abzuschließen und ein Ergebnis zu liefern.

[Stackoverflow](https://stackoverflow.com/questions/4130194/what-is-the-difference-between-task-and-thread)

Weiter gehts im `Boss.cs` -> Start()

## Programmausgabe

In der NLog.config ist das Pattern definiert in der die Ausgabe erfolgen soll.

Zeitpunkt | Thread Id | Loglevel | Name des Loggers > Ausgabe

Die einzigen Logger sind in Alice und dem Boss definiert. Bob und Sam erben von Alice, da der Code nicht unnötig vergrößert werden sollte.

```
2018-05-18 09:35:49.2612 | 6 | INFO | DamnWorker.src.Alice > Sam starting work...
2018-05-18 09:35:49.2612 | 5 | INFO | DamnWorker.src.Alice > Bob starting work...
2018-05-18 09:35:49.2612 | 4 | INFO | DamnWorker.src.Alice > Alice starting work...
2018-05-18 09:35:50.2914 | 4 | INFO | DamnWorker.src.Boss >      Alice at 1
2018-05-18 09:35:50.7899 | 5 | INFO | DamnWorker.src.Boss >        Bob at 1
2018-05-18 09:35:51.2924 | 4 | INFO | DamnWorker.src.Boss >      Alice at 2
2018-05-18 09:35:52.2900 | 5 | INFO | DamnWorker.src.Boss >        Bob at 2
2018-05-18 09:35:52.2900 | 4 | INFO | DamnWorker.src.Boss >      Alice at 3
2018-05-18 09:35:53.2896 | 6 | INFO | DamnWorker.src.Boss >        Sam at 1
2018-05-18 09:35:53.2896 | 4 | INFO | DamnWorker.src.Boss >      Alice at 4
2018-05-18 09:35:53.7902 | 5 | INFO | DamnWorker.src.Boss >        Bob at 3
2018-05-18 09:35:54.2930 | 4 | INFO | DamnWorker.src.Boss >      Alice at 5
2018-05-18 09:35:55.2905 | 5 | INFO | DamnWorker.src.Boss >        Bob at 4
2018-05-18 09:35:55.2905 | 4 | INFO | DamnWorker.src.Boss >      Alice at 6
2018-05-18 09:35:56.2935 | 4 | INFO | DamnWorker.src.Boss >      Alice at 7
2018-05-18 09:35:56.7906 | 5 | INFO | DamnWorker.src.Boss >        Bob at 5
2018-05-18 09:35:57.2898 | 6 | INFO | DamnWorker.src.Boss >        Sam at 2
2018-05-18 09:35:57.2937 | 4 | INFO | DamnWorker.src.Boss >      Alice at 8
2018-05-18 09:35:58.2908 | 5 | INFO | DamnWorker.src.Boss >        Bob at 6
2018-05-18 09:35:58.2908 | 4 | INFO | DamnWorker.src.Boss >      Alice at 9
2018-05-18 09:35:59.2942 | 4 | INFO | DamnWorker.src.Boss >      Alice at 10
2018-05-18 09:35:59.7911 | 5 | INFO | DamnWorker.src.Boss >        Bob at 7
2018-05-18 09:36:00.2945 | 4 | INFO | DamnWorker.src.Boss >      Alice at 11
2018-05-18 09:36:01.2901 | 6 | INFO | DamnWorker.src.Boss >        Sam at 3
2018-05-18 09:36:01.2901 | 5 | INFO | DamnWorker.src.Boss >        Bob at 8
2018-05-18 09:36:01.2947 | 4 | INFO | DamnWorker.src.Boss >      Alice at 12
2018-05-18 09:36:02.2949 | 4 | INFO | DamnWorker.src.Boss >      Alice at 13
2018-05-18 09:36:02.7915 | 5 | INFO | DamnWorker.src.Boss >        Bob at 9
2018-05-18 09:36:03.2952 | 4 | INFO | DamnWorker.src.Boss >      Alice at 14
 2018-05-18 09:36:04.2918 | 5 | INFO | DamnWorker.src.Alice > ... Bob stopped work
2018-05-18 09:36:04.2918 | 4 | INFO | DamnWorker.src.Alice > ... Alice stopped work
2018-05-18 09:36:05.2905 | 6 | INFO | DamnWorker.src.Alice > ... Sam stopped work
```

In der drittletzten Zeile ist ein Freizeichen enthalten. Dadurch wurde in `Program.cs` erfolgreich ein Key gelesen (`Console.ReadKey()`) und das Programm stoppt.