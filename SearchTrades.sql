CREATE TRIGGER SearchOutPutsTradeType
ON OutPuts
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar o último Trade na tabela OutPuts
    DECLARE @LastTrade VARCHAR(50);
    SELECT @LastTrade = MAX(Trade)
    FROM OutPuts;

    -- Verificar se existem registros na tabela OutPuts com os tipos de TradeType específicos e Trades entre 'trade1' e o último Trade
    IF EXISTS (
        SELECT 1
        FROM OutPuts
        WHERE TradeType IN ('HIGHRISK', 'MEDIUMRISK', 'LOWRISK', 'Uncaterogorized Risks')
        AND Trade BETWEEN 'trade1' AND @LastTrade
    )
    BEGIN
        -- Executar ações desejadas se a condição for atendida
     
        PRINT 'Registros encontrados na tabela OutPuts com os tipos de TradeType específicos e Trades entre "trade1" e o último Trade';
    END
END;
