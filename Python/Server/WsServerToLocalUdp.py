import asyncio
import websockets
import socket

async def handle_client(websocket, path):
    # Handle incoming messages from the client
    async for message in websocket:
        # Process the message
        print(f"Received message: {message}")

        # Send a response back to the client
        response = f"Server received: {message}"


        # Create a UDP socket
        udp_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

        # Send the response to the UDP port 3615
        udp_socket.sendto(message, ('localhost', 3615))

        # Close the UDP socket
        udp_socket.close()
        # await websocket.send(response)

async def start_server():
    # Create a WebSocket server
    server = await websockets.serve(handle_client, '0.0.0.0', 3615)


    # Start the server
    print("WebSocket server started")

    # Keep the server running until interrupted
    await server.wait_closed()

# Start the WebSocket server
asyncio.run(start_server())