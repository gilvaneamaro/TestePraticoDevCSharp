CREATE TABLE clientes (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    telefone VARCHAR(20),
    status INTEGER NOT NULL DEFAULT 1
);

CREATE TABLE produtos (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    descricao TEXT,
    preco NUMERIC(10,2) NOT NULL CHECK (preco > 0),
    estoque INT NOT NULL CHECK (estoque >= 0),
    ativo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE vendas (
    id SERIAL PRIMARY KEY,

    cliente_id INT NOT NULL,

    data_venda TIMESTAMP NOT NULL DEFAULT NOW(),

    metodo_pagamento INTEGER NOT NULL,

    valor_total NUMERIC(10,2) NOT NULL CHECK (valor_total >= 0),

    CONSTRAINT fk_vendas_cliente
        FOREIGN KEY (cliente_id)
        REFERENCES clientes(id),

    CONSTRAINT chk_metodo_pagamento
        CHECK (metodo_pagamento IN (1, 2, 3, 4, 5))
);

CREATE TABLE venda_itens (
    id SERIAL PRIMARY KEY,
    venda_id INT NOT NULL,
    produto_id INT NOT NULL,
    quantidade INT NOT NULL CHECK (quantidade > 0),
    preco_unitario NUMERIC(10,2) NOT NULL CHECK (preco_unitario > 0),
    FOREIGN KEY (venda_id) REFERENCES vendas(id),
    FOREIGN KEY (produto_id) REFERENCES produtos(id)
);


CREATE INDEX idx_vendas_cliente
ON vendas (cliente_id);

CREATE INDEX idx_vendas_data
ON vendas (data_venda);

CREATE INDEX idx_venda_itens_venda
ON venda_itens (venda_id);

CREATE INDEX idx_venda_itens_produto
ON venda_itens (produto_id);