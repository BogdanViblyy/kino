using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kino
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}
//Class
//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=kino;Integrated Security=True

//Home
//Data Source=HOME\SQLEXPRESS;Initial Catalog=kino;Integrated Security=True

//database
//create database kino;
//use kino;

//create table users(
//userId int primary key identity(1,1),
//userName varchar(50),
//password varchar(50));

//create table film(
//filmId int primary key identity(1,1),
//filmName varchar(50),
//filmYear int,
//filmImg text);

//create table seat(
//seatId int primary key identity(1,1),
//filmId int,
//seatStatus int,
//FOREIGN KEY (filmId) REFERENCES film(filmId));

//create table ticket(
//tickeId int primary key identity(1,1),
//seatId int,
//userId int,
//filmId int,
//FOREIGN KEY (seatId) REFERENCES seat(seatId),
//FOREIGN KEY (userId) REFERENCES users(userId),
//FOREIGN KEY (filmId) REFERENCES film(filmId));

//select* from users;
//select* from film;
//select* from seat;
//select* from ticket;

//use kino

//select * from users
//select * from film
//select * from seat

//insert into film (filmName, filmYear, filmImg) values
//('Зелёный слоник',1999,'https://avatars.mds.yandex.net/get-kino-vod-films-gallery/177294/8edaf90de249e40a839c77ced441aab8/380x240')

//drop table seat

//create table seat(
//seatId int primary key identity(1,1),
//filmName varchar(50),
//seatName varchar(50),
//seatStatus int)

//insert into seat (filmName, seatName, seatStatus) values
//('Borat','button1_1',0),
//('Borat', 'button1_2', 0),
//('Borat', 'button1_3', 0),
//('Borat', 'button1_4', 0),
//('Borat', 'button1_5', 0),
//('Borat', 'button2_1', 0),
//('Borat', 'button2_2', 0),
//('Borat', 'button2_3', 0),
//('Borat', 'button2_4', 0),
//('Borat', 'button2_5', 0),
//('Borat', 'button3_1', 0),
//('Borat', 'button3_2', 0),
//('Borat', 'button3_3', 0),
//('Borat', 'button3_4', 0),
//('Borat', 'button3_5', 0),
//('Borat', 'button4_1', 0),
//('Borat', 'button4_2', 0),
//('Borat', 'button4_3', 0),
//('Borat', 'button4_4', 0),
//('Borat', 'button4_5', 0),
//('Borat', 'button5_1', 0),
//('Borat', 'button5_2', 0),
//('Borat', 'button5_3', 0),
//('Borat', 'button5_4', 0),
//('Borat', 'button5_5', 0);

//insert into seat (filmName, seatName, seatStatus) values
//('Borat 2','button1_1',0),
//('Borat 2', 'button1_2', 0),
//('Borat 2', 'button1_3', 0),
//('Borat 2', 'button1_4', 0),
//('Borat 2', 'button1_5', 0),
//('Borat 2', 'button2_1', 0),
//('Borat 2', 'button2_2', 0),
//('Borat 2', 'button2_3', 0),
//('Borat 2', 'button2_4', 0),
//('Borat 2', 'button2_5', 0),
//('Borat 2', 'button3_1', 0),
//('Borat 2', 'button3_2', 0),
//('Borat 2', 'button3_3', 0),
//('Borat 2', 'button3_4', 0),
//('Borat 2', 'button3_5', 0),
//('Borat 2', 'button4_1', 0),
//('Borat 2', 'button4_2', 0),
//('Borat 2', 'button4_3', 0),
//('Borat 2', 'button4_4', 0),
//('Borat 2', 'button4_5', 0),
//('Borat 2', 'button5_1', 0),
//('Borat 2', 'button5_2', 0),
//('Borat 2', 'button5_3', 0),
//('Borat 2', 'button5_4', 0),
//('Borat 2', 'button5_5', 0);

//insert into seat (filmName, seatName, seatStatus) values
//('Зелёный слоник','button1_1',0),
//('Зелёный слоник', 'button1_2', 0),
//('Зелёный слоник', 'button1_3', 0),
//('Зелёный слоник', 'button1_4', 0),
//('Зелёный слоник', 'button1_5', 0),
//('Зелёный слоник', 'button2_1', 0),
//('Зелёный слоник', 'button2_2', 0),
//('Зелёный слоник', 'button2_3', 0),
//('Зелёный слоник', 'button2_4', 0),
//('Зелёный слоник', 'button2_5', 0),
//('Зелёный слоник', 'button3_1', 0),
//('Зелёный слоник', 'button3_2', 0),
//('Зелёный слоник', 'button3_3', 0),
//('Зелёный слоник', 'button3_4', 0),
//('Зелёный слоник', 'button3_5', 0),
//('Зелёный слоник', 'button4_1', 0),
//('Зелёный слоник', 'button4_2', 0),
//('Зелёный слоник', 'button4_3', 0),
//('Зелёный слоник', 'button4_4', 0),
//('Зелёный слоник', 'button4_5', 0),
//('Зелёный слоник', 'button5_1', 0),
//('Зелёный слоник', 'button5_2', 0),
//('Зелёный слоник', 'button5_3', 0),
//('Зелёный слоник', 'button5_4', 0),
//('Зелёный слоник', 'button5_5', 0);
