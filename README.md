# Jackhammer

## Description

Jackhammer is a cross-platform rhythm Stepmania-like game.

In fact, it is a 'reincarnation' of my university course work [Notemania](https://github.com/lunacys/Notemania).

Jackhammer is fully written in C# using [MonoGame Framework](https://github.com/mono/MonoGame).

Jackhammer will be available in Windows, Linux, OS X and Android systems.

## Platform support

- [x] Windows
- [ ] Linux
- [ ] OS X
- [ ] Android

## Building

To be able to build the Desktop project you need only [Visual Studio 2015](https://www.visualstudio.com/) or higher.

If you want to build Android project, you'll need to get [Visual Studio](https://www.visualstudio.com/) with Android Tools (Xamarin for Android).

First of all, you need to clone the repo:

```git clone https://github.com/lunacys/Jackhammer.git```

Next, open the ```Jackhammer.sln``` file and select the required solution configuration - Debug or Release.

You can now build and run the game.

### Deps

Following frameworks are used in the project as NuGet packages:

- ```Jackhammer.Framework``` - Game framework
- ```MonoGame``` - Main framework
- ```MonoGame.Extended``` - Screens, Viewport adapters
- ```Newton.Json``` - JSON Searialization/Deserialization
- ```NAudio``` - Audio, Music
- ```SoundTouch``` - Music Varispeed

## Resources

Currently all the game resources are in under work. They will be available as soon as possible.

Plans on game resorces:

- [ ] Website
- [ ] Discord Channel
- [ ] Game Wiki

## License

All the game source code is under [MIT](LICENSE.txt) license except for SoundTouch which is under LGPL v2.1 license. Check [LICENSE.txt](LICENSE.txt) file for more information.

## Описание

Jackhammer представляет собой игру наподобии Stepmania или osu!mania.
Фактически является продолжением моего проекта, сделанного для университетской практики
[Notemania](https://github.com/lunacys/Notemania). В том же репозитории есть подробное описание механики игры.

Главные особенности заключаются в следующем:

- Удобный красочный интерфейс.
- Возможность смены и редактирования скина (внешнего вида игры).
- Подробная статистика после каждой игры.
- Маскимальная сосредоточенность на процессе.
- Возможности для кастомизации процесса игры.
- Различные режимы для усложнения или упрощения процесса игры, делая его более интересным как для новичков, так и для профессионалов.
- Возможности для практикования игры и улучшения своих навыков.
- Версия под Windows, Linux, OS X и Android.
- Поддержка импорта карт из [osu!](https://osu.ppy.sh) (формат .osu).
- Возможность при помощи Drag&Drop изменять все элементы интерфейса.
- Автоматическая генерация карт.
- Автоматическое создание музыки из семплов и карт на её основе для тренировки различных паттернов.

### Техническая сторона

Любая карта, называемая Beatmap, сохраняется в трёх различных файлах: ```[mapname].jmap```, ```timing_points``` и ```hit_objects```. 

В первом файле хранится основная информация о карте: создатель трека, название трека, данные о сложности и т.д.

Во втором файле ```timing_points``` хранятся данные о тайминговых точках.

В третьем файле ```hit_objects``` хранится информация о всех объектах, существующих на карте.

Такое разделение используется для экономии места. К примеру, JSON файл, который содержит всю информацию, 
имеет размер в 6 раз больше, чем такие разделённые файлы.

Каждая карта хранится в своей папке в директории ```Maps```.

### TODO

Здесь указаны лишь цели. Прогресс их выполнения можно найти в файле [TODO](TODO).

- [ ] Технические вещи
  - [x] Система логгинга (Log) в файл и консоль с синхроном и асинхроном
  - [x] Система игровых установок (GameSettings), сериализация и десериализация
  - [x] Поддержка изменения скорости музыки и карты
  - [ ] Поддержка графиков (Graphs):
    - [ ] FpsGraph
    - [ ] AccuracyGraph
  - [ ] Логи, больше логов
  - [ ] Рефакторинг (особенно важно в GameplayScreen)
  - [ ] Поддержка анимации (игра + скины):
    - [ ] Элементов игрового поля (линии)
    - [ ] Кнопок
    - [ ] Нот (Click & Hold)
  - [ ] GUI:
    - [ ] Canvas
    - [ ] Button
    - [ ] Window
    - [ ] TextBox
    - [ ] Slider
    - [ ] ComboBox
  - [ ] Sprite fonts:
    - [ ] Для игры
    - [ ] Для скинов
  - [ ] Меню паузы
  - [ ] Главное меню
  - [ ] Подстраивание игры под экран (растягивание элементов интерфейса)
    - [ ] Разрешения
    - [ ] Соотношения сторон (16:9, 4:3, 16:10, 3:2)
  - [ ] Поддержка WaveForms для редактора
  - [ ] Возможность изменения положения элементов интерфейса и игрового процесса (Очки, Scoremeter, Игровое поле и т.д.)
  - [ ] Автоматическая генерация карты по музыке
  - [ ] Автоматические создание карт для тренировки различных паттернов
  - [ ] Генерация музыки для предыдущего пункта из семплов
- [ ] Загрузка/сохранение карт (Beatmap)
  - [x] Запись и чтение карты из файла в формате JSON
  - [ ] Загрузка списка всех карт в память при запуске игры
  - [ ] База данных, содержащая все индексированные карты для ускоренной загрузки списка карт
  - [ ] Изменение формата папки для сохранения карт: от ```[SongArtist] - [SongName]``` до ```[BeatmapSetID] [SongArtist] - [SongName]```
  - [ ] Импорт карт из osu!
- [ ] Игровой процесс:
  - [x] Отображение и прокрутка игровых объектов
  - [x] Возможность изменения игроком скоростью прокрутки карты
  - [x] Возможность изменения игроком режима прокрутки (сверху вниз или снизу вверх)
  - [x] Система скинов:
    - [x] Загрузка файлов изображений из нужной папки
    - [x] Файл, описывающий числовые значения и установки скина
  - [x] Загрузка и проигрывание музыки в формате ```.ogg```
  - [ ] Перемотка музыки в нужное положение
  - [ ] Игровое поле, содержащее от 4 до 7 линий в зависимости от сложности:
    - [x] 4k
    - [ ] 5k-7k
  - [x] Управление на заданные клавиши (по умолчанию Z X . / для 4 кнопок (4k))
  - [x] Настройка управления
  - [x] Поддержка ноты Click
  - [x] Scoremeter
  - [ ] Поддержка ноты Hold
  - [x] Система очков, точности и рангов
  - [ ] Поддержка HitSound и динамическое их изменение вместе с TimingPoints
  - [ ] Поддержка тайминговых точек в виде взаимодействия с игровым полем
  - [ ] Графики точности после прохождения карты
  - [ ] Игровые режимы:
    - [ ] Easy - больше порог точности (ниже OD)
    - [ ] Hard - меньше порог точности (выше OD)
    - [ ] DoubleTime - скорость трека увеличивается в полтора раза
- [ ] Редактор карт
- [ ] Версия под Linux
- [ ] Версия под Android:
  - [ ] Заточка управления под тач
  - [ ] Подстройка игры под экран смартфона
  - [ ] Подстройка редактора под тач
- [ ] Сайт
- [ ] Сервер
  - [ ] Сохранение очков игрока
  - [ ] Сохранение очков для кажой карты
- [ ] Соединение клиент-сервер-сайт
