CREATE TABLE INDICADORES_PLANTAS 
(
  ID_INDICADOR NUMBER NOT NULL 
, ID_PLANTA NUMBER NOT NULL 
, CONSTRAINT INDICADORES_PLANTAS_PK PRIMARY KEY 
  (
    ID_INDICADOR 
  , ID_PLANTA 
  )
  ENABLE 
);

ALTER TABLE INDICADORES_PLANTAS
ADD CONSTRAINT INDICADORES_PLANTAS_FK1 FOREIGN KEY
(
  ID_INDICADOR 
)
REFERENCES INDICADORES
(
  ID 
)
ENABLE;

COMMENT ON TABLE INDICADORES_PLANTAS IS 'Se especifican las plantas en las que se pintara cada indicador';

COMMENT ON COLUMN INDICADORES_PLANTAS.ID_INDICADOR IS 'Id del indicador';

COMMENT ON COLUMN INDICADORES_PLANTAS.ID_PLANTA IS 'Id de la planta';

ALTER TABLE INDICADORES_PLANTAS
ADD CONSTRAINT INDICADORES_PLANTAS_FK2 FOREIGN KEY
(
  ID_PLANTA 
)
REFERENCES PLANTAS
(
  ID 
)
ENABLE;


---------------------------------------------------------------------------

ALTER TABLE CIERRE_INDICADORES
ADD CONSTRAINT CIERRE_INDICADORES_FK1 FOREIGN KEY
(
  ID_USUARIO 
)
REFERENCES SAB.USUARIOS
(
  ID 
)
ENABLE;

ALTER TABLE CIERRE_INDICADORES
ADD CONSTRAINT CIERRE_INDICADORES_FK2 FOREIGN KEY
(
  ID_PLANTA 
)
REFERENCES PLANTAS
(
  ID 
)
ENABLE;

ALTER TABLE PLANTAS
ADD CONSTRAINT PLANTAS_FK2 FOREIGN KEY
(
  ID_MONEDA 
)
REFERENCES XBAT.COMON
(
  CODMON 
)
ENABLE;

begin
    for ind in (select id from indicadores) 
    loop     
        insert into indicadores_plantas(id_indicador,id_planta) values(ind.id,1);
        insert into indicadores_plantas(id_indicador,id_planta) values(ind.id,2);
        insert into indicadores_plantas(id_indicador,id_planta) values(ind.id,3);
        insert into indicadores_plantas(id_indicador,id_planta) values(ind.id,4);
        insert into indicadores_plantas(id_indicador,id_planta) values(ind.id,5);
        insert into indicadores_plantas(id_indicador,id_planta) values(ind.id,6);
        insert into indicadores_plantas(id_indicador,id_planta) values(ind.id,7);
        insert into indicadores_plantas(id_indicador,id_planta) values(ind.id,9);
		insert into indicadores_plantas(id_indicador,id_planta) values(ind.id,10);
    end loop;
end;