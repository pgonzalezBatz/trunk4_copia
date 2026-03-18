CREATE TABLE ANTICIPOS_TIPO_MOV 
(
  ID NUMBER NOT NULL 
, NOMBRE VARCHAR2(30) 
, CONSTRAINT ANTICIPOS_TIPO_MOV_PK PRIMARY KEY 
  (
    ID 
  )
  ENABLE 
);

COMMENT ON TABLE ANTICIPOS_TIPO_MOV IS 'Se guardan los posibles tipos';

COMMENT ON COLUMN ANTICIPOS_TIPO_MOV.ID IS 'Id del tipo movimiento';

COMMENT ON COLUMN ANTICIPOS_TIPO_MOV.NOMBRE IS 'Descripcion';

ALTER TABLE ANTICIPOS_TIPO_MOV 
ADD (DESCRIPCION VARCHAR2(40) );

ALTER TABLE ANTICIPOS_TIPO_MOV  
MODIFY (NOMBRE VARCHAR2(20 BYTE) );

COMMENT ON COLUMN ANTICIPOS_TIPO_MOV.NOMBRE IS 'Nombre';

COMMENT ON COLUMN ANTICIPOS_TIPO_MOV.DESCRIPCION IS 'Descripcion';

'Se meten los datos

Insert into BIDAIAK.ANTICIPOS_TIPO_MOV (ID,NOMBRE,DESCRIPCION) values ('1','Solicitud','Solicitud de un anticipo');
Insert into BIDAIAK.ANTICIPOS_TIPO_MOV (ID,NOMBRE,DESCRIPCION) values ('2','Entrega','Entrega del mismo');
Insert into BIDAIAK.ANTICIPOS_TIPO_MOV (ID,NOMBRE,DESCRIPCION) values ('3','Devolucion','Devolucion de dinero');
Insert into BIDAIAK.ANTICIPOS_TIPO_MOV (ID,NOMBRE,DESCRIPCION) values ('4','Transferencia','Transferencia a otro viaje');
Insert into BIDAIAK.ANTICIPOS_TIPO_MOV (ID,NOMBRE,DESCRIPCION) values ('5','Diferencia de cambio','Mete un movimiento para que quede a 0');
Insert into BIDAIAK.ANTICIPOS_TIPO_MOV (ID,NOMBRE,DESCRIPCION) values ('6','Hoja de gastos','Movimiento de hoja de gasto');
Insert into BIDAIAK.ANTICIPOS_TIPO_MOV (ID,NOMBRE,DESCRIPCION) values ('7','Manual','Movimiento manual metido por admin');
