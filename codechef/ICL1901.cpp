#include<bits/stdc++.h>
using namespace std;

#define int         long long
#define ull         unsigned long long
#define ll             long long
#define pb             push_back
#define all(a)      (a).begin(),(a).end()
#define sz(x)       (ll)x.size()
#define endl        '\n'
#define gcd(a,b)    __gcd((a),(b))
#define lcm(a,b)    ((a)*(b)) / gcd((a),(b))
#define ios            ios_base::sync_with_stdio(false);cin.tie(0);cout.tie(0);
#define mp             make_pair
#define lb             lower_bound
#define ub            upper_bound
#define F           first
#define S           second
#define rep(i, begin, end) for (__typeof(end) i = (begin) - ((begin) > (end)); i != (end) - ((begin) > (end)); i += 1 - 2 * ((begin) > (end)))
#define hell         (ull)1e9

void solve();

signed main(void)
{
    ios
    freopen("input.txt", "r", stdin);freopen("output.txt", "w", stdout);

    int TESTS=1;
    cin>>TESTS;

    while(TESTS--){
        solve();
    }
    return 0;
}


void solve()
{
    int N, K;
    cin >> K >> N;
    vector<bool> found(10,false);
    int cnt = 0;
    while(K > 0)
    {
        if(!found[K % 10])
        {
            cnt++;
        }
        found[K % 10] = true;
        K /= 10;
    }

    switch(cnt)
    {
        case 1:
            cout<<1<<"\n";
            break;
        case 2:
            cout<<8<<"\n";
            break;
        case 3:
            cout<<27<<"\n";
            break;
    }
}