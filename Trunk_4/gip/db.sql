ALTER TABLE prov_corp add id_usuario_administrador number;
COMMENT ON COLUMN prov_corp.id_usuario_administrador  IS 'Id usuario administrador de todos los usuarios de todos los proveedores en el corporativo';

create sequence seq_prov_corp start with 1 increment by 1 nocache
create table prov_corp(
	id		number not null,
	nombre		varchar2(150) not null,
       constraint pk_prov_corp primary key (id)
	
)
create table prov_corp_empresas(
	id_prov_corp		number not null,
	id_empresas		number not null,
	  constraint pk_prov_corp_empresas primary key (id_prov_corp, id_empresas),
       constraint fk_prov_corp__empresas foreign key (id_empresas) references sab.empresas(id),
	constraint fk_prov_corp__prov_corp foreign key(id_prov_corp) references sab.prov_corp(id)
)
--insertar recurso de 

insert into usuariosgrupos	

(select u.id,296
from usuarios u 
where	u.codpersona Is null And u.iddirectorioactivo Is null And (u.fechabaja Is null Or u.fechabaja < sysdate) And u.idplanta= 1 and idempresas<>1
and not exists (select 1
		from usuariosgrupos ug
	left outer join gruposrecursos gr on gr.idgrupos=ug.idgrupos 
	where  gr.idrecursos= 365 and u.id=ug.idusuarios))

alter table EMPRESAS_LOG add (email2 varchar2(50)) 
alter table EMPRESAS_LOG add (id_sab_cambio number)
alter table EMPRESAS_LOG modify (id_sab_cambio not null)

create table empresas_log(
       idempresas		number not null,
       fecha			date not null,
       email			varchar2(50),
       cif			varchar2(15),
       contacto			varchar2(50),
       direccion		varchar2(250),
       fecha_baja		date,
       id_fpago			number,
       id_pais			number,
       localidad		varchar2(50),
       nombre			varchar2(100),
       provincia		varchar2(50),
       telefono			varchar2(20),
       cpostal			varchar(20),
       id_sab_cambio		number not null,
       constraint pk_empresas_log primary key(idempresas, fecha)
)

grant select,insert,update,delete on gcprocom to sab
grant select,insert,update,delete on numeros_abreviados to sab
grant select on gctipiva to sab
grant  insert, update  on gcprovee to sab
grant select on numeros_abreviados to sab
grant select on GCPROPOT to sab

create table gctipiva(
       id		number not null,
       porcentaje	number not null,
       descripcion	varchar2(100),
       id_brain		varchar2(1),
       constraint pk_gctipiva primary key(id)
)

insert into gctipiva values(3, 0, 'Extranjero', '0')
insert into gctipiva values(4, 10, 'Reducido', '2')
insert into gctipiva values(5, 21, 'Normal', '1')

update copais set codpai_brain=code3, codpai_iva_brain='99'
update copais set codpai_brain='CN', codpai_iva_brain='CN' where codpai=2
update copais set codpai_iva_brain='IE' where codpai=17
update copais set codpai_brain='SDA', codpai_iva_brain='SD' where codpai=27
update copais set codpai_brain='NL', codpai_iva_brain='NL' where codpai=31
update copais set codpai_iva_brain='BE' where codpai=32
update copais set codpai_brain='F', codpai_iva_brain='FR' where codpai=33
update copais set codpai_brain='E', codpai_iva_brain='ES' where codpai=34
update copais set codpai_brain='P', codpai_iva_brain='PT' where codpai=35
update copais set codpai_brain='CZ', codpai_iva_brain='CZ' where codpai=37
update copais set codpai_brain='SI', codpai_iva_brain='SI' where codpai=38
update copais set codpai_brain='IT', codpai_iva_brain='IT' where codpai=39
update copais set codpai_brain='RO', codpai_iva_brain='RO' where codpai=40
update copais set codpai_brain='CH', codpai_iva_brain='CH' where codpai=41
update copais set codpai_brain='A', codpai_iva_brain='AT' where codpai=43
update copais set codpai_brain='GB', codpai_iva_brain='GB' where codpai=44
update copais set codpai_brain='HU', codpai_iva_brain='HU' where codpai=45
update copais set codpai_brain='S', codpai_iva_brain='SE' where codpai=46
update copais set codpai_brain='POL', codpai_iva_brain='PL' where codpai=47
update copais set codpai_brain='D', codpai_iva_brain='DE' where codpai=49
update copais set codpai_brain='MEX' where codpai=52
update copais set codpai_brain='ARG' where codpai=54
update copais set codpai_brain='BR' where codpai=55
update copais set codpai_brain='TH' where codpai=66
update copais set codpai_brain='COR' where codpai=82
update copais set codpai_brain='CHI' where codpai=86
update copais set codpai_brain='TR', codpai_iva_brain='TU' where codpai=90
update copais set codpai_brain='SK' where codpai=91
update copais set codpai_brain='AL' where codpai=101
update copais set codpai_brain='AND' where codpai=104
update copais set codpai_brain='BG' where codpai=128
update copais set codpai_brain='HR' where codpai=147
update copais set codpai_brain='EW' where codpai=159
update copais set codpai_brain='GR' where codpai=173
update copais set codpai_brain='LT' where codpai=209
update copais set codpai_brain='MC' where codpai=226
update copais set codpai_brain='MA' where codpai=230
update copais set codpai_brain='N' where codpai=245
update copais set codpai_brain='PR' where codpai=252
update copais set codpai_brain='PE' where codpai=253
update copais set codpai_brain='SC' where codpai=273
update copais set codpai_brain='CY' where codpai=149
update copais set codpai_brain='IS' where codpai=188
update copais set codpai_brain='FL' where codpai=208
update copais set codpai_brain='DK', codpai_iva_brain='DK' where codpai=150
update copais set codpai_brain='FIN', codpai_iva_brain='FI' where codpai=164
update copais set codpai_brain='LV', codpai_iva_brain='LV' where codpai=203
update copais set codpai_brain='L', codpai_iva_brain='LU' where codpai=210
update copais set codpai_brain='M', codpai_iva_brain='MT' where codpai=218



update comon set codmon_brain='USD' where codmon=2
update comon set codmon_brain='GBP' where codmon=4
update comon set codmon_brain='SFR' where codmon=6
update comon set codmon_brain='SKR' where codmon=10
update comon set codmon_brain='BRB' where codmon=11
update comon set codmon_brain='CZK' where codmon=12
update comon set codmon_brain='CDN' where codmon=14
update comon set codmon_brain='LIT' where codmon=17
update comon set codmon_brain='MXN' where codmon=18
update comon set codmon_brain='CNY' where codmon=16
update comon set codmon_brain='ZAR' where codmon=24
update comon set codmon_brain='EUR' where codmon=90
