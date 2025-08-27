-- Create Visit Status table
CREATE TABLE IF NOT EXISTS visit_status (
    id INT PRIMARY KEY,
    name VARCHAR(20) NOT NULL
);

-- Create Visit table
CREATE TABLE IF NOT EXISTS visit (
    id SERIAL PRIMARY KEY,
    date TIMESTAMP WITH TIME ZONE NOT NULL,
    address VARCHAR(50) NOT NULL,
    name VARCHAR(20) NOT NULL,
    phone VARCHAR(20) NOT NULL,
    visit_status_id INTEGER NOT NULL,
    create_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    is_deleted BOOLEAN NOT NULL DEFAULT FALSE,
    FOREIGN KEY (visit_status_id) REFERENCES visit_status(id)
);

-- Create Glass table
CREATE TABLE IF NOT EXISTS glass (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(100) NOT NULL,
    price_in_transparent DECIMAL(10,2) NULL,
    price_in_color DECIMAL(10,2) NULL,
    "order" SERIAL NOT NULL,
    is_deleted BOOLEAN NOT NULL DEFAULT FALSE,
    create_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    modify_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- Insert default visit status values if they don't exist
INSERT INTO visit_status (id, name) 
SELECT 1, 'pending' WHERE NOT EXISTS (SELECT 1 FROM visit_status WHERE name = 'pending');

INSERT INTO visit_status (id, name) 
SELECT 2, 'completed' WHERE NOT EXISTS (SELECT 1 FROM visit_status WHERE name = 'completed');

-- Insert default glass values if they don't exist
INSERT INTO glass (name, price_in_transparent, "order") 
SELECT 'Sencillo 2 mm', 21000, 1 WHERE NOT EXISTS (SELECT 1 FROM glass WHERE name = 'Sencillo 2 mm');

INSERT INTO glass (name, price_in_transparent, "order") 
SELECT 'Doble 3 mm', 25000, 2 WHERE NOT EXISTS (SELECT 1 FROM glass WHERE name = 'Doble 3 mm');

INSERT INTO glass (name, price_in_transparent, price_in_color, "order") 
SELECT 'Triple 4 mm', 30500, 51000, 3 WHERE NOT EXISTS (SELECT 1 FROM glass WHERE name = 'Triple 4 mm');

INSERT INTO glass (name, price_in_transparent, "order") 
SELECT 'Cristal 5 mm', 41000, 4 WHERE NOT EXISTS (SELECT 1 FROM glass WHERE name = 'Cristal 5 mm');

INSERT INTO glass (name, price_in_transparent, price_in_color, "order") 
SELECT 'Cristal 6 mm', 53000, 77000, 5 WHERE NOT EXISTS (SELECT 1 FROM glass WHERE name = 'Cristal 6 mm');

INSERT INTO glass (name, price_in_transparent, "order") 
SELECT 'Sencillo difuso 2 mm (A.R)', 34000, 6 WHERE NOT EXISTS (SELECT 1 FROM glass WHERE name = 'Sencillo difuso 2 mm (A.R)');

INSERT INTO glass (name, price_in_transparent, "order") 
SELECT 'Stop sol 4 mm (Espejado)', 147000, 7 WHERE NOT EXISTS (SELECT 1 FROM glass WHERE name = 'Stop sol 4 mm (Espejado)');

INSERT INTO glass (name, price_in_transparent, "order") 
SELECT 'Fantasia 4 mm', 42000, 8 WHERE NOT EXISTS (SELECT 1 FROM glass WHERE name = 'Fantasia 4 mm');

INSERT INTO glass (name, price_in_transparent, "order") 
SELECT 'Armado 6 mm', 89000, 9 WHERE NOT EXISTS (SELECT 1 FROM glass WHERE name = 'Armado 6 mm');

INSERT INTO glass (name, price_in_color, "order") 
SELECT 'Martele 4 mm', 72000, 10 WHERE NOT EXISTS (SELECT 1 FROM glass WHERE name = 'Martele 4 mm');

-- Create index on Date for better query performance
CREATE INDEX IX_Visit_Date ON visit (Date);
