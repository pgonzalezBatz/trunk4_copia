ALTER TABLE TARIFAS_LIN 
ADD (TARIFA_COCHE NUMBER );

COMMENT ON COLUMN TARIFAS_LIN.TARIFA_COCHE IS 'Tarifa del coche de alquiler';

-----------------------------------------------------------------------------------
UPDATE TARIFAS_LIN SET TARIFA_COCHE=0 WHERE 1=1