# Asteroider
Clone of Asteroids game.<br />
Тестовое задание для Kefir! на должность Unity Developer.<br />
Версия Unity 2021.3.5f1<br />

Запуск со сцены "Preload".

Не использовались сторонние фреймворки.<br />
Не использовалась физика Unity.<br />
Используется новая Input System с генерацией C# скрипта.<br />
Для UI текста используется TextMeshPro.<br />
Сохранение рекорда в json файл.<br />
Используются Assembly Definitions.

Использовались следующие паттерны:
* Ecs
* DI container
* Object Pool
* State Machine

# Настройки
Все настройки лежат в Assets/Settings
1. AsteroiderSettings: общие настройки игры.
2. AsteroiderBattleConfiguration: настройки геймплея.
3. AsteroiderLayerCollisionMatrix: настройки столкновения слоёв для игровой физики.
4. AsteroiderInputActionAsset: настройки ввода Input System.
