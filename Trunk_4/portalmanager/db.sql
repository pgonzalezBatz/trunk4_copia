alter table solicitud add datos_incorporacion varchar2(300)

--consolidacion de puesto
insert into pregunta (select id + 21, titulo,descripcion,tipo_pregunta, peso, 2 from pregunta where id<22 and id not in(11,12,13))
insert into pregunta values(43,'Otras','Indica las funciones que está desempeñando',2,null,2)
insert into pregunta values(44,'Otras','¿Necesitaría alguna formación para mejorar su desempeño en el puesto?',2,null,2)
insert into pregunta values(45,'Otras','¿Recomiendas su continuidad en este nuevo puesto?',2,null,2)

insert into pregunta (select id + 45, titulo,descripcion,tipo_pregunta, peso, 3 from pregunta where id<22 and id not in(11,12,13))
insert into pregunta values(,'Otras','Indica las funciones que está desempeñando',2,null,3)
insert into pregunta values(,'Otras','¿Necesitaría alguna formación para mejorar su desempeño en el puesto?',2,null,3)
insert into pregunta values(,'Otras','¿Recomiendas su consolidación en el nuevo puesto? En caso positivo indica la propuesta de indice. En caso negativo indica el motivo por el que no se consolida.',2,null,3)

create table tipos_formulario(
       id		number not null,
       descripcion	varchar2(50) not null,
       constraint pk_tipos_formulario primary key(id)
)
insert into tipos_formulario values(1,'Evaluaciones')
insert into tipos_formulario values(2,'Cambio puesto')
insert into tipos_formulario values(3,'Consolidación puesto')

alter table pregunta add id_tipo_formulario number
alter table respuesta add id_tipo_formulario number

update pregunta set id_tipo_formulario=1
update respuesta set id_tipo_formulario=1

create table consolidacion_cambio_puesto(
	id		number not null,
	fecha_vencimiento	date not null,
	id_sab		number not null,
	continua		number not null,
	motivo		varchar2(400),
	indice		number,
	constraint pk_consolidacion_cp primary key(id),
       constraint fk_usuarios_consolidacion_cp foreign key(id_sab) references sab.usuarios(id)
)



grant select on gruposrecursos to eki;
grant select on usuariosgrupos to eki;
grant select on usuarios_plantas to eki;
grant select on usuarios to eki;
grant select,delete,insert on tickets to eki;

drop table recursos;
drop table validaciones;
drop table solicitudes_idiomas;
drop table solicitudes_idiomas_complement;
drop table solicitadores;
drop table validadores;
drop table asignaciones;
drop table solicitudes;

create table solicitud(
       id			number not null,
       id_sab			number not null,
       nPersonas		number not null,
       negocio			varchar2(50) not null,
       departamento		varchar2(100) not null,
       responsable		number not null,
       descripcion		varchar2(500),
       especialidad		varchar2(100),
       conocimientos		varchar2(200),
       idiomas			varchar2(100),
       experiencia		varchar2(100),
       duracion			varchar2(200),
       fecha_inicio		date,
       horario			varchar2(30),		       
       fecha_incorporacion	date,
       fecha_creacion_registro  date,
       constraint pk_solicitud primary key(id),
       constraint fk_usuarios_solicitud foreign key(id_sab) references sab.usuarios(id)
)
create table cobertura_puesto(
       id_solicitud		number not null,
       plan_gestion		number not null,
       puesto_estructural	number not null,
       puesto			varchar2(100),
       formacion		varchar2(100),	
       formacion2		varchar2(100),
       conocimientos2		varchar2(100),
       idiomas2			varchar2(100),
       experiencia2		varchar2(100),
       constraint pk_cobertura_puesto primary key(id_solicitud),
       constraint fk_cobertura_solicitud foreign key(id_solicitud) references solicitud(id)
)

create table becaria(
       id_solicitud	number not null,
       universidad	varchar2(100),
       titulacion	varchar2(100),
       constraint pk_becaria primary key(id_solicitud),
       constraint fk_becaria_solicitud foreign key(id_solicitud) references solicitud(id)
)

create table solicitud_responsable(
       id_solicitud		number not null,
       id_sab			number not null,
       orden			number not null,
       fecha_validacion		date,
       fecha_rechazo		date,
       constraint pk_responsable primary key(id_solicitud,id_sab),
       constraint fk_solicitud_responsable foreign key(id_solicitud) references solicitud(id),
       constraint fk_usuarios_responsable foreign key(id_sab) references sab.usuarios(id)
       
)

create table pregunta(
       id			number not null,
       titulo			nvarchar2(100) not null,
       descripcion		nvarchar2(500) not null,
       tipo_pregunta		number not null,
       peso			number,
       constraint pk_pregunta primary key(id)
)
create table respuesta_posible(
       id_pregunta		number not null,
       id			number not null,
       puntuacion		number not null,
       descripcion		nvarchar2(300) not null,
       constraint pk_respuesta_posible primary key(id),
       constraint fk_respuesta_posible_pregunta foreign key(id_pregunta) references pregunta(id)
)
create table respuesta(
       id			number not null,
       id_sab			number not null,
       fecha			date not null,
       titulo_pregunta		nvarchar2(100) not null,
       descripcion_pregunta	nvarchar2(500) not null,
       tipo_pregunta		number not null,
       peso_pregunta		number,
       puntuacion    		number,
       puntuacion_max		number,
       texto			nvarchar2(400),
       fecha_vencimiento	date not null,
       id_sab_creador		number not null,
       constraint pk_respuesta primary key(id),
       constraint fk_respuesta_usuarios foreign key(id_sab) references sab.usuarios(id)	
)

create table propuesta_continuidad (
       id			number not null,
       fecha_vencimiento	date not null,
       id_sab			number not null,
       continua			number(1) not null,
       duracion			varchar2(50),
       indice			number,
       motivo			nvarchar2(300),
       constraint pk_propuesta_continuidad primary key(id),
       constraint fk_prop_continuidad_usuarios foreign key(id_sab) references sab.usuarios(id)
)

create table notificado_vencimiento(
       id			number not null,
       id_sab			integer not null,
       fecha_vencimiento	date not null,
       fecha_notificacion	date not null,
       constraint pk_notificado_vencimiento primary key(id),
       constraint fk_notif_vencimiento_usuarios foreign key(id_sab) references sab.usuarios(id)
)

create sequence solicitud_seq start with 1 increment by 1    nocache
create sequence pregunta_seq start with 1 increment by 1    nocache
create sequence respuesta_posible_seq start with 1 increment by 1    nocache
create sequence respuesta_seq start with 1 increment by 1    nocache
create sequence propuesta_continuidad_seq start with 1 increment by 1    nocache
create sequence notificado_vencimiento_seq start with 1 increment by 1 nocache
