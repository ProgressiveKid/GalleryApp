# Описание
Предметной областью проекта является онлайн галерея, в которой пользователи могут добавлять свои изображения.
 Данный проект решает проблему хранения изображении на локальном устройстве, а также позволяет управлять данынми и обмениваться ими между клиентскими устройствами и сервером

Серверная часть спроектирована как REST WEB API приложение. Для работы с БД на сервере используется ORM Entity Framework, также реализовано: логгирование для выявления ошибок, кэширование данных для более быстрой работы с бд, сервис по работе с галереeй покрыт модульными тестами для обеспечения надёжности работы серверной части проекта. 

Клиентское WPF приложение предоставляет функционал для просмотра ранее отправленных данных в виде списка, где каждая запись содержит текст и соответствующее изображение. Приложение поддерживает возможность сохранения данных на сервере, чтобы пользователи могли получить доступ к ним при последующих сеансах, а также редактирование и удаление записей. Использует архитектуру MVVM, 

# Пример работы
## Навигация в приложении представлена вкладками: 
![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/7edaa127-fbbf-4329-80ae-0c803cd4985d)
## 1) Форма добавления нового изображения в галерею:
![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/1f87d0eb-88fc-4506-b7f8-95b98ed1f843)
### форма после загрузки изображения
![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/fea1923b-0c11-481b-89f0-eb31fcc83639)
## Сообщение об отправки на сервер
![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/85e9baf6-407f-4efe-aa74-5718f89f4dd6)
## Присутсвует валидация на добавление нового элемента галереи: <br>
### на размер изображения ![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/41fe36ba-d3ac-4c3c-aea9-f436ca454345) <br>
### на длину описания изображения ![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/1a488387-7fe1-42d1-bb19-a402188bdf49) <br>


## 2) Форма просмотра всех изображении в галереии:
### при входе на форму будет загружен список всех изображении 
![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/2dbc6b74-26de-49de-ac0d-3efac6cfda25)
### после выбора из списка названия изображения - соответсвующее изображение будет показано на экране, также после выбора становятся доступными кнопки: редактировать и удалить выбранный элемент галлереи 
![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/d44f1502-0a66-4b15-a410-47dffeff9246)
![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/c7d4822f-0e08-4f3d-b1b1-239c090fa09b)
### После обновления мы получаем актуальный список изображении
![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/a1d7945a-ca56-4a69-af1c-a8628b57060e)


## 3) Форма описания проекта:
![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/be5dd86d-feca-4a64-8043-31c0b6ab789b)

# Примечание
## *Для корректной работы клиента - должен работать сервер, при тестировании рекомендуется запускать оба проекта
