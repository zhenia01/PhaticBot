scrollToBottom = () => {
    const chat = document.getElementsByClassName("messages-content")[0];
    chat.scrollTo(0, chat.scrollHeight);
    
    document.getElementsByClassName("message-input")[0].focus();
}