#Image ready to  connect to linux azure vm using ssh via key
# Use a lightweight Linux base image
FROM debian:bullseye

# Set the working directory
WORKDIR /root

# Copy the SSH key into the image
# Replace 'master-node_key.pem' with the actual file name
COPY master-node_key.pem /root/master-node_key.pem

# Set the correct permissions for the key
RUN chmod 600 /root/master-node_key.pem

# Install the SSH client
RUN apt-get update && \
    apt-get install -y openssh-client && \
    apt-get clean

# Set a default command to keep the container running
CMD ["bash"]
