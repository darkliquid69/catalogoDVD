delimiter #

drop procedure if exists sp_ListarPorPais#
create procedure sp_ListarPorPais(in p char(2))
comment 'Listado de dvd filtrado por pais'
begin
	select * from dvd where pais = p;
end#

delimiter ;
