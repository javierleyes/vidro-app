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

-- Create index on Date for better query performance
CREATE INDEX IX_Visit_Date ON visit (Date);
