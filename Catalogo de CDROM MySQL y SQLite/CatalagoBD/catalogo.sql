create database if not exists catalogo;
use catalogo;

drop table if exists dvd;

create table dvd (
	codigo smallint,
	titulo varchar(40) not null,
	artista varchar(30) not null,
	pais char(2) not null default '??',
	compania varchar(40) not null,
	precio decimal(6,2),	
	anio smallint
);



