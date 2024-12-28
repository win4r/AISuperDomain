using System.Text.Json;

namespace Aila;

public class ConfigurationManager
{
    public delegate Task DisplayAlertDelegate(string title, string message, string cancel);

    public DisplayAlertDelegate OnDisplayAlertRequested;
    
    
    public async Task InitializeDefaultConfigurationAsync()
    {
        var filePath = Path.Combine(FileSystem.AppDataDirectory, "config.txt");

        // 检查文件是否已存在
        if (!File.Exists(filePath))
        {
            // 默认的JSON内容
            string defaultJson = @"

{
  ""AiConfig"": [
    {
      ""id"": 1,
      ""name"": ""ChatGPT"",
      ""url"": ""https://chat.openai.com/"",
      ""script"": ""(function() { var textarea = document.querySelector('textarea'); textarea.value = '[message]'; textarea.dispatchEvent(new Event('input', {bubbles: true})); setTimeout(function() { var sendButton = document.querySelector('[data-testid=\""send-button\""]'); if (sendButton) sendButton.click(); }, 1000); })();"",
      ""iniScript"": ""alert(0)""
    },
    {
      ""id"": 2,
      ""name"": ""Gemini"",
      ""url"": ""https://gemini.google.com/"",
      ""script"": ""(function(p, btn) { p.innerText = '[message]', setTimeout(function() { btn.click(); }, 500); })(document.querySelector('.ql-editor.textarea > p'), document.querySelector('.send-button.mdc-icon-button'));"",
      ""iniScript"": """"
    },
    {
      ""id"": 3,
      ""name"": ""Copilot"",
      ""url"": ""https://www.bing.com/search?q=Bing+AI&showconv=1&FORM=hpcodx"",
      ""script"": ""(document.querySelector('cib-serp').shadowRoot.querySelector('#cib-action-bar-main').shadowRoot.querySelector('cib-text-input').shadowRoot.querySelector('#searchbox').value = '[message]', document.querySelector('cib-serp').shadowRoot.querySelector('#cib-action-bar-main').shadowRoot.querySelector('cib-text-input').shadowRoot.querySelector('#searchbox').dispatchEvent(new Event('input', {bubbles: true })), document.querySelector('cib-serp').shadowRoot.querySelector('#cib-action-bar-main').shadowRoot.querySelector('cib-text-input').shadowRoot.querySelector('#searchbox').dispatchEvent(new KeyboardEvent('keydown', {key: 'Enter' })));"",
      ""iniScript"": """"
    },
    {
      ""id"": 4,
      ""name"": ""Claude"",
      ""url"": ""https://claude.ai/chats"",
      ""script"": ""(function(){document.querySelector('.is-empty.is-editor-empty').innerHTML = '[message]'; setTimeout(function(){var elem = document.querySelector('.ProseMirror p'); if (elem) {var event = new KeyboardEvent('keydown', {'key': 'Enter', 'code': 'Enter', 'keyCode': 13, 'which': 13, 'bubbles': true, 'cancelable': true}); setTimeout(function(){elem.dispatchEvent(event);}, 500); setTimeout(function(){var buttonElement = document.querySelector('button[aria-label=\""Send Message\""]'); if (buttonElement) {buttonElement.click();}}, 1000);}}, 1000);}());"",
      ""iniScript"": """"
    },
    {
      ""id"": 5,
      ""name"": ""HuggingChat"",
      ""url"": ""https://huggingface.co/chat/"",
      ""script"": ""(document.querySelector('textarea').value = '[message]', document.querySelector('textarea').dispatchEvent(new Event('input', {bubbles: true })), document.querySelector('textarea').dispatchEvent(new KeyboardEvent('keydown', {key: 'Enter' })))"",
      ""iniScript"": """"
    },
    {
      ""id"": 6,
      ""name"": ""Perplexity"",
      ""url"": ""https://www.perplexity.ai/"",
      ""script"": ""(function(){let a=document.querySelector('textarea'),b=Object.getOwnPropertyDescriptor(window.HTMLTextAreaElement.prototype,\""value\"").set;b.call(a,'[message]');let c=new Event('input',{bubbles:true});a.dispatchEvent(c);let d=new KeyboardEvent('keydown',{bubbles:true,key:'Enter',keyCode:13});a.dispatchEvent(d)})();"",
      ""iniScript"": """"
    },
    {
      ""id"": 7,
      ""name"": ""YouAI"",
      ""url"": ""https://you.com/"",
      ""script"": ""(function(){const e=document.querySelector('textarea[aria-label=\""Query bar\""][data-testid=\""qbInput\""]'),t='[message]';if(!e)return console.error('Textarea not found');const n=Object.getOwnPropertyDescriptor(window.HTMLTextAreaElement.prototype,\""value\"").set;n.call(e,t),e.dispatchEvent(new Event('input',{bubbles:!0})),setTimeout(()=>{e.dispatchEvent(new KeyboardEvent('keydown',{bubbles:!0,cancelable:!0,keyCode:13,key:'Enter',code:'Enter',shiftKey:!1}))},500)})();"",
      ""iniScript"": """"
    },
    {
      ""id"": 8,
      ""name"": ""lmsys"",
      ""url"": ""https://chat.lmsys.org/"",
      ""script"": ""(function() { document.querySelectorAll('button.svelte-kqij2n').length >= 3 && document.querySelectorAll('button.svelte-kqij2n')[2].click(); document.querySelectorAll('.svelte-1ed2p3z').forEach(function(e) { e.style.display = 'none'; }); document.querySelectorAll('textarea[data-testid=\""textbox\""]').forEach(function(t) { t.value = '[message]'; t.dispatchEvent(new Event('input', { bubbles: true })); }); setTimeout(function() { [document.querySelector('#component-28'), document.querySelector('#component-65'), document.querySelector('#component-93')].forEach(function(b) { b && b.click(); }); }, 1000); })();"",
      ""iniScript"": """"
    },
    {
      ""id"": 9,
      ""name"": ""Assistant"",
      ""url"": ""https://poe.com/Assistant"",
      ""script"": ""((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', {bubbles: true }), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }), textarea.dispatchEvent(enterKeyEvent)));"",
      ""iniScript"": """"
    },
    {
      ""id"": 10,
      ""name"": ""Poe-ChatGPT"",
      ""url"": ""https://poe.com/ChatGPT"",
      ""script"": ""((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', {bubbles: true }), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }), textarea.dispatchEvent(enterKeyEvent)));"",
      ""iniScript"": """"
    },
    {
      ""id"": 11,
      ""name"": ""Poe-Claude-3-Sonnet"",
      ""url"": ""https://poe.com/Claude-3-Sonnet"",
      ""script"": ""((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', {bubbles: true }), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }), textarea.dispatchEvent(enterKeyEvent)));"",
      ""iniScript"": """"
    },
    {
      ""id"": 12,
      ""name"": ""Poe-Claude-instant-100k"",
      ""url"": ""https://poe.com/Claude-instant-100k"",
      ""script"": ""((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', {bubbles: true }), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }), textarea.dispatchEvent(enterKeyEvent)));"",
      ""iniScript"": """"
    },
    {
      ""id"": 13,
      ""name"": ""Web-Search"",
      ""url"": ""https://poe.com/Web-Search"",
      ""script"": ""((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', {bubbles: true }), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }), textarea.dispatchEvent(enterKeyEvent)));"",
      ""iniScript"": """"
    },
    {
      ""id"": 14,
      ""name"": ""Gemini-Pro"",
      ""url"": ""https://poe.com/Gemini-Pro"",
      ""script"": ""((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', {bubbles: true }), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }), textarea.dispatchEvent(enterKeyEvent)));"",
      ""iniScript"": """"
    },
    {
      ""id"": 15,
      ""name"": ""Poe-Qwen-通义千问"",
      ""url"": ""https://poe.com/Qwen-72b-Chat"",
      ""script"": ""((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', {bubbles: true }), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }), textarea.dispatchEvent(enterKeyEvent)));"",
      ""iniScript"": """"
    },
    {
      ""id"": 16,
      ""name"": ""together-gpt3.5"",
      ""url"": ""https://api.together.xyz/playground/chat/openchat/openchat-3.5-1210"",
      ""script"": ""(function simulateInputChangeAndEnter() { const input = document.querySelector('textarea[data-cy=\""input-textarea\""]'); const newValue = '[message]'; if (!input) { return console.error('Textarea not found'); } const nativeInputValueSetter = Object.getOwnPropertyDescriptor(window.HTMLTextAreaElement.prototype, \""value\"").set; nativeInputValueSetter.call(input, newValue); const inputEvent = new Event('input', { bubbles: true }); input.dispatchEvent(inputEvent); setTimeout(() => { const keydownEvent = new KeyboardEvent('keydown', { bubbles: true, cancelable: true, keyCode: 13, key: 'Enter', code: 'Enter', shiftKey: false }); input.dispatchEvent(keydownEvent); }, 500); })();"",
      ""iniScript"": """"
    },
    {
      ""id"": 17,
      ""name"": ""together-Llama"",
      ""url"": ""https://api.together.xyz/playground/chat/meta-llama/Llama-2-70b-chat-hf"",
      ""script"": ""(function simulateInputChangeAndEnter() { const input = document.querySelector('textarea[data-cy=\""input-textarea\""]'); const newValue = '[message]'; if (!input) { return console.error('Textarea not found'); } const nativeInputValueSetter = Object.getOwnPropertyDescriptor(window.HTMLTextAreaElement.prototype, \""value\"").set; nativeInputValueSetter.call(input, newValue); const inputEvent = new Event('input', { bubbles: true }); input.dispatchEvent(inputEvent); setTimeout(() => { const keydownEvent = new KeyboardEvent('keydown', { bubbles: true, cancelable: true, keyCode: 13, key: 'Enter', code: 'Enter', shiftKey: false }); input.dispatchEvent(keydownEvent); }, 500); })();"",
      ""iniScript"": """"
    },
    {
      ""id"": 18,
      ""name"": ""together-CodeLlama"",
      ""url"": ""https://api.together.xyz/playground/chat/codellama/CodeLlama-70b-Instruct-hf"",
      ""script"": ""(function simulateInputChangeAndEnter() { const input = document.querySelector('textarea[data-cy=\""input-textarea\""]'); const newValue = '[message]'; if (!input) { return console.error('Textarea not found'); } const nativeInputValueSetter = Object.getOwnPropertyDescriptor(window.HTMLTextAreaElement.prototype, \""value\"").set; nativeInputValueSetter.call(input, newValue); const inputEvent = new Event('input', { bubbles: true }); input.dispatchEvent(inputEvent); setTimeout(() => { const keydownEvent = new KeyboardEvent('keydown', { bubbles: true, cancelable: true, keyCode: 13, key: 'Enter', code: 'Enter', shiftKey: false }); input.dispatchEvent(keydownEvent); }, 500); })();"",
      ""iniScript"": """"
    },
    {
      ""id"": 19,
      ""name"": ""together-Qwen(通义千问)"",
      ""url"": ""https://api.together.xyz/playground/chat/Qwen/Qwen1.5-72B-Chat"",
      ""script"": ""(function simulateInputChangeAndEnter() { const input = document.querySelector('textarea[data-cy=\""input-textarea\""]'); const newValue = '[message]'; if (!input) { return console.error('Textarea not found'); } const nativeInputValueSetter = Object.getOwnPropertyDescriptor(window.HTMLTextAreaElement.prototype, \""value\"").set; nativeInputValueSetter.call(input, newValue); const inputEvent = new Event('input', { bubbles: true }); input.dispatchEvent(inputEvent); setTimeout(() => { const keydownEvent = new KeyboardEvent('keydown', { bubbles: true, cancelable: true, keyCode: 13, key: 'Enter', code: 'Enter', shiftKey: false }); input.dispatchEvent(keydownEvent); }, 500); })();"",
      ""iniScript"": """"
    },
    {
      ""id"": 20,
      ""name"": ""CharacterAI"",
      ""url"": ""https://beta.character.ai/chats"",
      ""script"": ""(function(){let a=document.querySelector('textarea'),b=Object.getOwnPropertyDescriptor(window.HTMLTextAreaElement.prototype,\""value\"").set;b.call(a,'[message]');let c=new Event('input',{bubbles:true});a.dispatchEvent(c);let d=new KeyboardEvent('keydown',{bubbles:true,key:'Enter',keyCode:13});a.dispatchEvent(d);})();document.querySelector('.aspect-square.h-10').click();"",
      ""iniScript"": """"
    },
    {
      ""id"": 21,
      ""name"": ""Poe-Photo_CreateE"",
      ""url"": ""https://poe.com/Photo_CreateE"",
      ""script"": ""((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', {bubbles: true }), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }), textarea.dispatchEvent(enterKeyEvent)));"",
      ""iniScript"": """"
    },
    {
      ""id"": 22,
      ""name"": ""Poe-Image_Creators"",
      ""url"": ""https://poe.com/Image_Creators"",
      ""script"": ""((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', {bubbles: true }), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }), textarea.dispatchEvent(enterKeyEvent)));"",
      ""iniScript"": """"
    },
    {
      ""id"": 23,
      ""name"": ""Poe-StableDiffusionXL"",
      ""url"": ""https://poe.com/StableDiffusionXL"",
      ""script"": ""((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', {bubbles: true }), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }), textarea.dispatchEvent(enterKeyEvent)));"",
      ""iniScript"": """"
    },
    {
      ""id"": 24,
      ""name"": ""Poe-Playground"",
      ""url"": ""https://poe.com/Playground-v2.5"",
      ""script"": ""((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', {bubbles: true }), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }), textarea.dispatchEvent(enterKeyEvent)));"",
      ""iniScript"": """"
    },
    {
      ""id"": 25,
      ""name"": ""Poe-Photo-Realism"",
      ""url"": ""https://poe.com/Photo-Realism"",
      ""script"": ""((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', {bubbles: true }), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }), textarea.dispatchEvent(enterKeyEvent)));"",
      ""iniScript"": """"
    },
    {
      ""id"": 26,
      ""name"": ""Meta image"",
      ""url"": ""https://imagine.meta.com/"",
      ""script"": ""(function() { let textarea = document.querySelector('textarea'); let nativeInputValueSetter = Object.getOwnPropertyDescriptor(window.HTMLTextAreaElement.prototype, \""value\"").set; nativeInputValueSetter.call(textarea, '[message]'); let inputEvent = new Event('input', { bubbles: true }); textarea.dispatchEvent(inputEvent); let enterEvent = new KeyboardEvent('keydown', { bubbles: true, key: 'Enter', keyCode: 13 }); textarea.dispatchEvent(enterEvent); })();"",
      ""iniScript"": """"
    },
    {
      ""id"": 27,
      ""name"": ""doubao(豆包)"",
      ""url"": ""https://www.doubao.com/chat/"",
      ""script"": ""(function(){ let a=document.querySelector('div#root textarea'),b=Object.getOwnPropertyDescriptor(window.HTMLTextAreaElement.prototype,'value').set;b.call(a,'[message]');a.dispatchEvent(new Event('input',{ bubbles:true }));a.dispatchEvent(new KeyboardEvent('keydown',{ bubbles:true,key:'Enter',keyCode:13 })); })();"",
      ""iniScript"": """"
    }
  ],
  ""CurrentAi"": [
    {""id"":1 },
    {""id"":2 },
    {""id"":3 },
    {""id"": 6},
    {""id"": 7},
    {""id"": 5},
    {""id"": 9}
  ],
  ""Prompts"": [
    {
      ""id"": 1,
      ""title"": ""English Translator and Improver"",
      ""prompt"": ""I want you to act as an English translator, spelling corrector and improver. I will speak to you in any language and you will detect the language, translate it and answer in the corrected and improved version of my text, in English. I want you to replace my simplified A0-level words and sentences with more beautiful and elegant, upper level English words and sentences. Keep the meaning same, but make them more literary. I want you to only reply the correction, the improvements and nothing else, do not write explanations. My first sentence is \n""
    },
    {
      ""id"": 2,
      ""title"": ""position Interviewer"",
      ""prompt"": ""I want you to act as an interviewer. I will be the candidate and you will ask me the interview questions for the position position. I want you to only reply as the interviewer. Do not write all the conservation at once. I want you to only do the interview with me. Ask me the questions and wait for my answers. Do not write explanations. Ask me the questions one by one like an interviewer does and wait for my answers. My first sentence is \n""
    },
    {
      ""id"": 3,
      ""title"": ""Excel Sheet"",
      ""prompt"": ""I want you to act as a text based excel. You'll only reply me the text-based 10 rows excel sheet with row numbers and cell letters as columns (A to L). First column header should be empty to reference row number. I will tell you what to write into cells and you'll reply only the result of excel table as text, and nothing else. Do not write explanations. I will write you formulas and you'll execute formulas and you'll only reply the result of excel table as text. First, reply me the empty sheet.\n""
    },
    {
      ""id"": 4,
      ""title"": ""Spoken English Teacher"",
      ""prompt"": ""I want you to act as a spoken English teacher and improver. I will speak to you in English and you will reply to me in English to practice my spoken English. I want you to keep your reply neat, limiting the reply to 100 words. I want you to strictly correct my grammar mistakes, typos, and factual errors. I want you to ask me a question in your reply. Now let's start practicing, you could ask me a question first. Remember, I want you to strictly correct my grammar mistakes, typos, and factual errors.\n""
    },
    {
      ""id"": 5,
      ""title"": ""Plagiarism Checker"",
      ""prompt"": ""I want you to act as a plagiarism checker. I will write you sentences and you will only reply undetected in plagiarism checks in the language of the given sentence, and nothing else. Do not write explanations on replies. My first sentence is \n""
    },
    {
      ""id"": 6,
      ""title"": ""Character from 'Movie/Book/Anything"",
      ""prompt"": ""I want you to act like {character} from {series}. I want you to respond and answer like {character} using the tone, manner and vocabulary {character} would use. Do not write any explanations. Only answer like {character}. You must know all of the knowledge of {character}. My first sentence is \n""
    },
    {
      ""id"": 7,
      ""title"": ""Advertiser"",
      ""prompt"": ""I want you to act as an advertiser. You will create a campaign to promote a product or service of your choice. You will choose a target audience, develop key messages and slogans, select the media channels for promotion, and decide on any additional activities needed to reach your goals. My first suggestion request is \n""
    },
    {
      ""id"": 8,
      ""title"": ""Storyteller"",
      ""prompt"": ""I want you to act as a storyteller. You will come up with entertaining stories that are engaging, imaginative and captivating for the audience. It can be fairy tales, educational stories or any other type of stories which has the potential to capture people's attention and imagination. Depending on the target audience, you may choose specific themes or topics for your storytelling session e.g., if it’s children then you can talk about animals; If it’s adults then history-based tales might engage them better etc. My first request is \n""
    },
    {
      ""id"": 9,
      ""title"": ""Motivational Coach"",
      ""prompt"": ""I want you to act as a motivational coach. I will provide you with some information about someone's goals and challenges, and it will be your job to come up with strategies that can help this person achieve their goals. This could involve providing positive affirmations, giving helpful advice or suggesting activities they can do to reach their end goal. My first request is \n""
    },
    {
      ""id"": 10,
      ""title"": ""Composer"",
      ""prompt"": ""I want you to act as a composer. I will provide the lyrics to a song and you will create music for it. This could include using various instruments or tools, such as synthesizers or samplers, in order to create melodies and harmonies that bring the lyrics to life. My first request is \n""
    },
    {
      ""id"": 11,
      ""title"": ""Screenwriter"",
      ""prompt"": ""I want you to act as a screenwriter. You will develop an engaging and creative script for either a feature length film, or a Web Series that can captivate its viewers. Start with coming up with interesting characters, the setting of the story, dialogues between the characters etc. Once your character development is complete - create an exciting storyline filled with twists and turns that keeps the viewers in suspense until the end. My first request is \n""
    },
    {
      ""id"": 12,
      ""title"": ""Novelist"",
      ""prompt"": ""I want you to act as a novelist. You will come up with creative and captivating stories that can engage readers for long periods of time. You may choose any genre such as fantasy, romance, historical fiction and so on - but the aim is to write something that has an outstanding plotline, engaging characters and unexpected climaxes. My first request is \n""
    },
    {
      ""id"": 13,
      ""title"": ""Movie Critic"",
      ""prompt"": ""I want you to act as a movie critic. You will develop an engaging and creative movie review. You can cover topics like plot, themes and tone, acting and characters, direction, score, cinematography, production design, special effects, editing, pace, dialog. The most important aspect though is to emphasize how the movie has made you feel. What has really resonated with you. You can also be critical about the movie. Please avoid spoilers. My first request is \n""
    },
    {
      ""id"": 14,
      ""title"": ""AI Writing Tutor"",
      ""prompt"": ""I want you to act as an AI writing tutor. I will provide you with a student who needs help improving their writing and your task is to use artificial intelligence tools, such as natural language processing, to give the student feedback on how they can improve their composition. You should also use your rhetorical knowledge and experience about effective writing techniques in order to suggest ways that the student can better express their thoughts and ideas in written form. My first request is \n""
    },
    {
      ""id"": 15,
      ""title"": ""Second Example Title"",
      ""prompt"": ""Second Example Prompt Text""
    },
    {
      ""id"": 16,
      ""title"": ""UX/UI Developer"",
      ""prompt"": ""I want you to act as a UX/UI developer. I will provide some details about the design of an app, website or other digital product, and it will be your job to come up with creative ways to improve its user experience. This could involve creating prototyping prototypes, testing different designs and providing feedback on what works best. My first request is \n""
    },
    {
      ""id"": 17,
      ""title"": ""Cyber Security Specialist"",
      ""prompt"": ""I want you to act as a cyber security specialist. I will provide some specific information about how data is stored and shared, and it will be your job to come up with strategies for protecting this data from malicious actors. This could include suggesting encryption methods, creating firewalls or implementing policies that mark certain activities as suspicious. My first request is \n""
    },
    {
      ""id"": 18,
      ""title"": ""Career Counselor"",
      ""prompt"": ""I want you to act as a career counselor. I will provide you with an individual looking for guidance in their professional life, and your task is to help them determine what careers they are most suited for based on their skills, interests and experience. You should also conduct research into the various options available, explain the job market trends in different industries and advice on which qualifications would be beneficial for pursuing particular fields. My first request is \n""
    },
    {
      ""id"": 19,
      ""title"": ""Design Consultant"",
      ""prompt"": ""I want you to act as a web design consultant. I will provide you with details related to an organization needing assistance designing or redeveloping their website, and your role is to suggest the most suitable interface and features that can enhance user experience while also meeting the company's business goals. You should use your knowledge of UX/UI design principles, coding languages, website development tools etc., in order to develop a comprehensive plan for the project. My first request is \n""
    },
    {
      ""id"": 20,
      ""title"": ""Financial Analyst"",
      ""prompt"": ""Want assistance provided by qualified individuals enabled with experience on understanding charts using technical analysis tools while interpreting macroeconomic environment prevailing across world consequently assisting customers acquire long term advantages requires clear verdicts therefore seeking same through informed predictions written down precisely! First statement contains following content \n""
    }
  ],
  ""ViewsCount"": {
    ""VCount"": 2
  }
}


";

            try
            {
                // 写入默认的JSON配置到文件
                await File.WriteAllTextAsync(filePath, defaultJson);
                if (OnDisplayAlertRequested != null)
                {
                    await OnDisplayAlertRequested.Invoke("Initialization", "Default configuration has been initialized.", "OK");
                }
            }
            catch (Exception ex)
            {
                if (OnDisplayAlertRequested != null)
                {
                    await OnDisplayAlertRequested.Invoke("Error", $"Failed to initialize default configuration: {ex.Message}", "OK");
                }
            }
        }
    }

    public async Task<AppConfiguration?> LoadConfigurationAsync()
    {
        var filePath = Path.Combine(FileSystem.AppDataDirectory, "config.txt");

        if (!File.Exists(filePath))
        {
            if (OnDisplayAlertRequested != null)
            {
                await OnDisplayAlertRequested.Invoke("Error", "Configuration file not found.", "OK");
            }
            return null;
        }

        try
        {
            var jsonContent = await File.ReadAllTextAsync(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Handle case sensitivity
            };
            var configuration = JsonSerializer.Deserialize<AppConfiguration>(jsonContent, options);

            if (configuration == null)
            {
                if (OnDisplayAlertRequested != null)
                {
                    await OnDisplayAlertRequested.Invoke("Error", "Failed to deserialize the configuration.", "OK");
                }
            }

            return configuration;
        }
        catch (JsonException ex)
        {
            if (OnDisplayAlertRequested != null)
            {
                await OnDisplayAlertRequested.Invoke("JSON Error", ex.Message, "OK");
            }
            return null;
        }
        catch (Exception ex)
        {
            if (OnDisplayAlertRequested != null)
            {
                await OnDisplayAlertRequested.Invoke("Error", ex.Message, "OK");
            }
            return null;
        }
    }
    
    public async Task SaveConfigurationAsync(AppConfiguration configuration)
    {
        var filePath = Path.Combine(FileSystem.AppDataDirectory, "config.txt");
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true // 使生成的JSON文件更易于阅读
        };
        var jsonContent = JsonSerializer.Serialize(configuration, options);

        try
        {
            await File.WriteAllTextAsync(filePath, jsonContent);
            if (OnDisplayAlertRequested != null)
            {
                await OnDisplayAlertRequested.Invoke("Success", "Configuration saved successfully.", "OK");
            }
        }
        catch (Exception ex)
        {
            if (OnDisplayAlertRequested != null)
            {
                await OnDisplayAlertRequested.Invoke("Error", $"Failed to save configuration: {ex.Message}", "OK");
            }
        }
    }

}
