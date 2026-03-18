CREATE TABLE CUESTIONARIO 
(
  ID_VIAJE NUMBER NOT NULL 
, PREGUNTA_1 NUMBER NOT NULL 
, TEXTO_1 VARCHAR2(150) 
, PREGUNTA_2 NUMBER NOT NULL 
, PREGUNTA_3 NUMBER NOT NULL 
, FECHA DATE NOT NULL 
, CONSTRAINT CUESTIONARIO_PK PRIMARY KEY 
  (
    ID_VIAJE 
  )
  ENABLE 
);

ALTER TABLE CUESTIONARIO
ADD CONSTRAINT CUESTIONARIO_FK1 FOREIGN KEY
(
  ID_VIAJE 
)
REFERENCES VIAJES
(
  ID 
)
ENABLE;

COMMENT ON TABLE CUESTIONARIO IS 'Cuestionario que se les envia a los planificadores, para que valoren la atencion de la agencia';

COMMENT ON COLUMN CUESTIONARIO.ID_VIAJE IS 'Id del viaje';

COMMENT ON COLUMN CUESTIONARIO.PREGUNTA_1 IS '1: Nada satisfactorio,2:Poco satisfactorio,3:Normal,4:Satisfactorio,5:My satisfactorio,6:NS/NC';

COMMENT ON COLUMN CUESTIONARIO.TEXTO_1 IS 'Texto opcional relacionado con la pregunta 1';

COMMENT ON COLUMN CUESTIONARIO.PREGUNTA_2 IS '1: Nada satisfactorio,2:Poco satisfactorio,3:Normal,4:Satisfactorio,5:My satisfactorio,6:NS/NC';

COMMENT ON COLUMN CUESTIONARIO.PREGUNTA_3 IS '1: Nada satisfactorio,2:Poco satisfactorio,3:Normal,4:Satisfactorio,5:My satisfactorio,6:NS/NC';

COMMENT ON COLUMN CUESTIONARIO.FECHA IS 'Fecha de la respuesta';
