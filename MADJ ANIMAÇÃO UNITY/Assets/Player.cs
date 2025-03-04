using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public float JumpForce;
    public float Speed;

    public bool isJumping;
    public bool isAttacking;
    public AudioSource somJump;
    public AudioSource somWalk;
    public AudioSource somAttack;

    private Rigidbody2D rig;
    private Animator anim;


    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Attack(); // Chama o método de ataque
    }

    void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f);
        transform.position += movement * Time.deltaTime * Speed;

        if (horizontalInput > 0f)
        {
            // Define a direção para a direita
            transform.eulerAngles = new Vector3(0f, 0f, 0f);

            // Tocar som de caminhada, se ainda não estiver tocando
            if (!somWalk.isPlaying)
            {
                somWalk.Play();
            }

            // Ativar animação de caminhada
            anim.SetBool("walk", true); 
        }
        else if (horizontalInput < 0f)
        {
            // Define a direção para a esquerda
            transform.eulerAngles = new Vector3(0f, 180f, 0f);

            // Tocar som de caminhada, se ainda não estiver tocando
            if (!somWalk.isPlaying)
            {
                somWalk.Play();
            }

            // Ativar animação de caminhada
            anim.SetBool("walk", true);
        }
        else
        {
            // Parar o som de caminhada se o jogador parar de se mover
            if (somWalk.isPlaying)
            {
                somWalk.Stop();
            }

            // Desativar animação de caminhada
            anim.SetBool("walk", false);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
                isJumping = true;
                anim.SetBool("Jump", true);
                somJump.Play();
            }
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z)) // Verifica se o jogador pode atacar
        {
            if (!isAttacking)
            {
                isAttacking = true; // O jogador está atacando
                anim.SetBool("Attack", true); // Ativar animação de ataque
                somAttack.Play(); // Tocar som de ataque
            }
        }
    }

    // Método que será chamado quando a animação de ataque terminar
    public void OnAttackAnimationEnd()
    {
        isAttacking = false; // Permitir novos ataques após a animação terminar
        anim.SetBool("Attack", false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            isJumping = false;
            anim.SetBool("Jump", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            isJumping = true;
        }
    }
}
