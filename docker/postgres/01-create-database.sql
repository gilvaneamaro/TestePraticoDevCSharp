CREATE TABLE clientes (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    telefone VARCHAR(20)
);

CREATE TABLE produtos (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    descricao TEXT,
    preco NUMERIC(10,2) NOT NULL CHECK (preco > 0),
    estoque INT NOT NULL CHECK (estoque >= 0)
);

CREATE TABLE vendas (
    id SERIAL PRIMARY KEY,
    cliente_id INT NOT NULL,
    data_venda TIMESTAMP NOT NULL DEFAULT NOW(),
    FOREIGN KEY (cliente_id) REFERENCES clientes(id)
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
