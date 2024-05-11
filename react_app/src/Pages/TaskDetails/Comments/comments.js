import React, {useEffect, useRef, useState} from "react";
import "./comments.css"
import axios from "axios";
import {getAuthToken} from "../../../Services/auth";
export const CommentsAndReplies = (props) => {
    const { token, user } = getAuthToken();

    const [reply, setReply] = useState({
        loading: true,
        result: [],
        err: null,
    });

    const replyForm = useRef({
        replyContent:""
    });


    const handleAddReply = (e) => {
        e.preventDefault();
        setReply({ ...reply, loading: true });

        axios.post(`https://localhost:7157/Opsphere/reply/${props.commentId}`, {
            replyContent: replyForm.current.commentContent.value,
        }, {
            headers: {
                Authorization: `Bearer ${token}`,
            }
        })
            .then(() => {
                replyForm.current.commentContent.value = "";
                setReply({ ...reply, loading: false });
                props.refreshComments()
            })
            .catch((errors) => {
                setReply({ ...reply, loading: false, err: [{ msg: `something went wrong` }] });
            });
    };


    return (
        <>
            <div className="comments">
                {props.userName} <br/>
                {props.commentContent}<br/>
                <div className="addComment1">
                    <form className="d-flex">
                        <input className="form-control me-2" type="addcomment"
                               placeholder="Type comment" aria-label="comment" ref={(val) => {
                            replyForm.current.commentContent = val;
                        }}/>
                        <button className="btn btn-primary" type="submit" onClick={handleAddReply}
                                style={{backgroundColor: 'rgb(81, 143, 143)'}}>submit
                        </button>
                    </form>
                </div>
                <button className="btn btn-primary" type="submit"
                        style={{backgroundColor: 'rgb(81, 143, 143)'}}>Replies
                </button>

                <div className="replies-container">
                    <hr style={{border: 'solid'}}/>
                    {props.replies.map((reply, index) => (
                        <div key={index}>
                            <strong>{reply.user.username}:</strong> {reply.replyContent}
                        </div>
                    ))}
                </div>
            </div>
            <a href="#test1" id="back">
                <button className="btn btn-primary" type="submit"
                        style={{backgroundColor: 'rgb(81, 143, 143)'}}>Top
                </button>
            </a>
        </>
    );
};
