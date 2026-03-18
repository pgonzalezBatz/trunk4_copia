ALTER TABLE MOVIMIENTOS 
ADD (CAMBIO_MONEDA NUMBER );

COMMENT ON COLUMN MOVIMIENTOS.CAMBIO_MONEDA IS 'Se guarda el cambio de la moneda usado para esa conversion a euros';

ALTER TABLE LINEAS_HOJA_GASTOS 
ADD (CAMBIO_MONEDA NUMBER );

COMMENT ON COLUMN LINEAS_HOJA_GASTOS.CAMBIO_MONEDA IS 'Se guarda el cambio de la moneda usado para esa conversion a euros';

