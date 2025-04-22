// Current user info
const currentUserId = '@ViewBag.CurrentUserId';
const currentUserName = '@ViewBag.CurrentUserName';

// Selected user info
let selectedUserId = null;
let selectedUserName = null;

// SignalR connection
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

// DOM elements
const messagesContainer = document.getElementById('messages-container');
const messageInput = document.getElementById('message-input');
const sendButton = document.getElementById('send-button');
const selectedUserNameElement = document.getElementById('selected-user-name');

// Start SignalR connection
connection.start().catch(err => console.error(err));

// Handle receiving messages
connection.on("ReceiveMessage", (senderId, message) => {
    if (senderId === selectedUserId) {
        appendMessage(message, 'received');
    }
});

connection.on("MessageSent", (receiverId, message) => {
    if (receiverId === selectedUserId) {
        appendMessage(message, 'sent');
    }
});

// User selection
document.querySelectorAll('.user-item').forEach(item => {
    item.addEventListener('click', () => {
        selectedUserId = item.dataset.userId;
        selectedUserName = item.dataset.userName;

        // Update UI
        document.querySelectorAll('.user-item').forEach(u => u.classList.remove('active'));
        item.classList.add('active');
        selectedUserNameElement.textContent = selectedUserName;

        // Enable chat input
        messageInput.disabled = false;
        sendButton.disabled = false;

        // Clear messages and load chat history
        messagesContainer.innerHTML = '';
        loadChatHistory(selectedUserId);
    });
});

// Send message
sendButton.addEventListener('click', () => {
    const message = messageInput.value.trim();
    if (message && selectedUserId) {
        connection.invoke("SendMessage", selectedUserId, message).catch(err => console.error(err));
        messageInput.value = '';
    }
});

messageInput.addEventListener('keypress', (e) => {
    if (e.key === 'Enter') {
        sendButton.click();
    }
});

// Helper functions
function appendMessage(message, type) {
    const messageElement = document.createElement('div');
    messageElement.className = `message ${type} mb-2 p-2`;
    messageElement.style.maxWidth = '70%';
    messageElement.style.borderRadius = '10px';
    messageElement.style.backgroundColor = type === 'sent' ? '#dcf8c6' : '#f1f0f0';
    messageElement.style.alignSelf = type === 'sent' ? 'flex-end' : 'flex-start';
    messageElement.textContent = message;

    const wrapper = document.createElement('div');
    wrapper.className = 'd-flex';
    wrapper.style.justifyContent = type === 'sent' ? 'flex-end' : 'flex-start';
    wrapper.appendChild(messageElement);

    messagesContainer.appendChild(wrapper);
    messagesContainer.scrollTop = messagesContainer.scrollHeight;
}

async function loadChatHistory(userId) {
    try {
        const response = await fetch(`/Chat/GetChatHistory?userId=${userId}`);
        const messages = await response.json();

        messages.forEach(msg => {
            const type = msg.senderId === currentUserId ? 'sent' : 'received';
            appendMessage(msg.content, type);
        });
    } catch (error) {
        console.error('Error loading chat history:', error);
    }
}