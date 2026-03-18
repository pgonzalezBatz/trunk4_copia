create table recuperar_pwd(
       nombreusuario	varchar2() not null,
       fecha		date not null,
       rui		varchar2(128) not null,
       constraint pk_recuperar_pwd primary key(nombreusuario, rui)
)
