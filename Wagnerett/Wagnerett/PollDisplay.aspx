﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="PollDisplay.aspx.cs" Inherits="Wagnerett.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/js/polldisplay.js"></script>
    <link href="/css/poll.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="PollBox">
        <div class="Question">Do you bees?</div>
    </div>
</asp:Content>
