--En Xbat
grant update on gclinped to  soldadura
--En SEGIPE
insert into estados values(17,'No compensar',1)
insert into estados values(18,'No compensar provisional',1)

alter table informe_proveedor add numpedcab number
alter table informe_proveedor add validado number(1,0)
alter table informes modify tipoinforme varchar2(20)

grant select on gccrigru to soldadura
grant select on gcarticu  to soldadura
grant select on gclinped  to soldadura
grant select on scpedcab to soldadura
grant select on scpedlin to soldadura
grant select on empresas to soldadura
grant select,references on usuarios to soldadura
grant select,insert,delete on tickets to soldadura
grant select on usuariosgrupos to soldadura
grant select on gruposrecursos to soldadura


ALTER TABLE informe_proveedor DROP CONSTRAINT pk_informe_proveedor
ALTER TABLE informe_proveedor DROP CONSTRAINT fk_informeproveedor_usuarios
ALTER TABLE informe_proveedor ADD CONSTRAINT pk_informe_proveedor primary key(id_informe)

alter table informes  add constraint PK_informes primary key (idinforme)
create table informe_proveedor(
       id_informe		number(9,0) not null,
       id_sab			number(9,0) not null,
       constraint pk_informe_proveedor primary key(id_informe),
       constraint fk_informeproveedor_usuarios foreign key(id_sab) references sab.usuarios(id),
       constraint fk_informeproveedor_usuariosinformes foreign key(id_informe) references informes(idinforme)
       )


