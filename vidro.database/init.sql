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
    price DECIMAL(10,2) NOT NULL,
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
INSERT INTO glass (name, price) 
SELECT 'Clear Glass', 15.50 WHERE NOT EXISTS (SELECT 1 FROM glass WHERE name = 'Clear Glass');

INSERT INTO glass (name, price) 
SELECT 'Tempered Glass', 25.75 WHERE NOT EXISTS (SELECT 1 FROM glass WHERE name = 'Tempered Glass');

INSERT INTO glass (name, price) 
SELECT 'Laminated Glass', 35.00 WHERE NOT EXISTS (SELECT 1 FROM glass WHERE name = 'Laminated Glass');

INSERT INTO glass (name, price) 
SELECT 'Frosted Glass', 22.30 WHERE NOT EXISTS (SELECT 1 FROM glass WHERE name = 'Frosted Glass');

INSERT INTO glass (name, price) 
SELECT 'Tinted Glass', 28.90 WHERE NOT EXISTS (SELECT 1 FROM glass WHERE name = 'Tinted Glass');

-- Create index on Date for better query performance
CREATE INDEX IX_Visit_Date ON visit (Date);
